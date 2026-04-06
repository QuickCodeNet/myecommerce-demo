using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuickCode.MyecommerceDemo.Common.Auditing;

public interface IAuditLogWriter
{
    Task QueueAuditLogAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
    
    Task QueueAuditLogsAsync(IEnumerable<AuditLog> auditLogs, CancellationToken cancellationToken = default);
}