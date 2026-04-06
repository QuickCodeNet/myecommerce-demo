using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.Common.Auditing;

public class AuditDbContext : DbContext
{
    public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AUDIT_LOGS");

            entity.HasIndex(e => new { e.EntityName, e.EntityId, e.Timestamp })
                .HasDatabaseName("IX_AuditLogs_Entity_Time");
            
            entity.HasIndex(e => new { e.UserId, e.Timestamp })
                .HasDatabaseName("IX_AuditLogs_User_Time");
            
            entity.HasIndex(e => e.Timestamp)
                .HasDatabaseName("IX_AuditLogs_Timestamp");
            
            var databaseProvider = Database.ProviderName;
            
            if (databaseProvider?.Contains("SqlServer") == true)
            {
                entity.HasIndex(e => e.CorrelationId)
                    .HasDatabaseName("IX_AuditLogs_CorrelationId")
                    .HasFilter("[CORRELATION_ID] IS NOT NULL");
            }
            else if (databaseProvider?.Contains("Npgsql") == true || databaseProvider?.Contains("PostgreSQL") == true)
            {
                entity.HasIndex(e => e.CorrelationId)
                    .HasDatabaseName("IX_AuditLogs_CorrelationId")
                    .HasFilter("\"CORRELATION_ID\" IS NOT NULL");
            }
            else
            {
                entity.HasIndex(e => e.CorrelationId)
                    .HasDatabaseName("IX_AuditLogs_CorrelationId");
            }
            
            if (databaseProvider?.Contains("SqlServer") == true)
            {
                entity.Property(e => e.OldValues).HasColumnType("nvarchar(max)");
                entity.Property(e => e.NewValues).HasColumnType("nvarchar(max)");
            }
            else if (databaseProvider?.Contains("Npgsql") == true || databaseProvider?.Contains("PostgreSQL") == true)
            {
                entity.Property(e => e.OldValues).HasColumnType("text");
                entity.Property(e => e.NewValues).HasColumnType("text");
            }
            else if (databaseProvider?.Contains("MySql") == true)
            {
                entity.Property(e => e.OldValues).HasColumnType("longtext");
                entity.Property(e => e.NewValues).HasColumnType("longtext");
            }
        });
    }
}