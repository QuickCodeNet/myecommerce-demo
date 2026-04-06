using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace QuickCode.MyecommerceDemo.Common.Auditing;

public class BackgroundAuditLogWriter : BackgroundService, IAuditLogWriter
{
    private readonly ILogger<BackgroundAuditLogWriter> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentQueue<AuditLog> _auditQueue = new();
    private readonly SemaphoreSlim _signal = new(0);
    private const int BatchSize = 100;
    private const int MaxQueueSize = 10000;
    
    public BackgroundAuditLogWriter(
        ILogger<BackgroundAuditLogWriter> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task QueueAuditLogAsync(AuditLog auditLog, CancellationToken cancellationToken = default)
    {
        if (_auditQueue.Count >= MaxQueueSize)
        {
            _logger.LogWarning("Audit queue is full. Dropping audit log for {EntityName}:{EntityId}", 
                auditLog.EntityName, auditLog.EntityId);
            return Task.CompletedTask;
        }
        
        auditLog.Hash = ComputeHash(auditLog);
        
        _auditQueue.Enqueue(auditLog);
        _signal.Release();
        
        return Task.CompletedTask;
    }

    public Task QueueAuditLogsAsync(IEnumerable<AuditLog> auditLogs, CancellationToken cancellationToken = default)
    {
        foreach (var auditLog in auditLogs)
        {
            if (_auditQueue.Count >= MaxQueueSize)
            {
                _logger.LogWarning("Audit queue is full. Stopping batch enqueue.");
                break;
            }

            auditLog.Hash = ComputeHash(auditLog);
            _auditQueue.Enqueue(auditLog);
        }
        
        _signal.Release();
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Audit Log Background Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _signal.WaitAsync(stoppingToken);

                var batch = new List<AuditLog>();
                while (batch.Count < BatchSize && _auditQueue.TryDequeue(out var auditLog))
                {
                    batch.Add(auditLog);
                }

                if (batch.Any())
                {
                    await WriteBatchAsync(batch, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing audit logs");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Retry delay
            }
        }
        
        await FlushRemainingLogsAsync(stoppingToken);
        
        _logger.LogInformation("Audit Log Background Service stopped");
    }

    private async Task WriteBatchAsync(List<AuditLog> batch, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        
        try
        {
            var auditContext = scope.ServiceProvider.GetService<AuditDbContext>();
            
            if (auditContext != null)
            {
                // Database'e batch insert
                await auditContext.AuditLogs.AddRangeAsync(batch, cancellationToken);
                await auditContext.SaveChangesAsync(cancellationToken);
                
                _logger.LogDebug("Successfully wrote {Count} audit logs to database", batch.Count);
            }
            else
            {
                // AuditDbContext yoksa sadece log'a yaz (fallback)
                _logger.LogWarning("AuditDbContext not configured. Writing to logs only.");
                
                foreach (var log in batch)
                {
                    _logger.LogInformation("AUDIT: {EntityName}:{EntityId} - {Action} by {UserId} at {Timestamp}", 
                        log.EntityName, log.EntityId, log.Action, log.UserId, log.Timestamp);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write audit log batch. Batch size: {Count}", batch.Count);
            
            var firstLog = batch.FirstOrDefault();
            if (firstLog != null && !firstLog.ErrorMessage?.Contains("RETRY_") == true)
            {
                foreach (var log in batch)
                {
                    log.ErrorMessage = $"RETRY_1: {ex.Message}";
                    _auditQueue.Enqueue(log);
                }
            }
            else if (firstLog?.ErrorMessage?.Contains("RETRY_1") == true)
            {
                foreach (var log in batch)
                {
                    log.ErrorMessage = $"RETRY_2: {ex.Message}";
                    _auditQueue.Enqueue(log);
                }
            }
            else
            {
                _logger.LogCritical(ex, "AUDIT LOG LOST after retries: {Batch}", 
                    System.Text.Json.JsonSerializer.Serialize(batch));
            }
        }
    }

    private async Task FlushRemainingLogsAsync(CancellationToken cancellationToken)
    {
        var remaining = new List<AuditLog>();
        while (_auditQueue.TryDequeue(out var auditLog))
        {
            remaining.Add(auditLog);
        }

        if (remaining.Any())
        {
            _logger.LogInformation("Flushing {Count} remaining audit logs", remaining.Count);
            await WriteBatchAsync(remaining, cancellationToken);
        }
    }
    
    private static string ComputeHash(AuditLog auditLog)
    {
        var data = $"{auditLog.EntityName}|{auditLog.EntityId}|{auditLog.Action}|" +
                   $"{auditLog.UserId}|{auditLog.Timestamp:O}|{auditLog.OldValues}|{auditLog.NewValues}";
        
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hashBytes);
    }
}