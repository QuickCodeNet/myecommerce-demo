using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.Common.Auditing;

[Table("AUDIT_LOGS")]
[Index(nameof(EntityName), nameof(EntityId), nameof(Timestamp))]
[Index(nameof(UserId), nameof(Timestamp))]
[Index(nameof(Timestamp))]
public class AuditLog
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Column("ENTITY_NAME")]
    [Required]
    [StringLength(250)]
    public string EntityName { get; set; } = string.Empty;
    
    [Column("ENTITY_ID")]
    [Required]
    [StringLength(250)]
    public string EntityId { get; set; } = string.Empty;
    
    [Column("ACTION")]
    [Required]
    [StringLength(50)]
    public string Action { get; set; } = string.Empty;
    
    [Column("USER_ID")]
    [StringLength(250)]
    public string? UserId { get; set; }
    
    [Column("USER_NAME")]
    [StringLength(250)]
    public string? UserName { get; set; }
    
    [Column("USER_GROUP")]
    [StringLength(250)]
    public string? UserGroup { get; set; }
    
    [Column("TIMESTAMP")]
    public DateTime Timestamp { get; set; }
    
    [Column("OLD_VALUES")]
    public string? OldValues { get; set; }
    
    [Column("NEW_VALUES")]
    public string? NewValues { get; set; }
    
    [Column("CHANGED_COLUMNS")]
    [StringLength(4000)]
    public string? ChangedColumns { get; set; }
    
    [Column("IS_CHANGED")]
    public bool IsChanged { get; set; }
    
    [Column("CHANGE_SUMMARY")]
    public string? ChangeSummary { get; set; }
    
    [Column("IP_ADDRESS")]
    [StringLength(45)] 
    public string? IpAddress { get; set; }
    
    [Column("USER_AGENT")]
    [StringLength(500)]
    public string? UserAgent { get; set; }
    
    [Column("CORRELATION_ID")]
    [StringLength(100)]
    public string? CorrelationId { get; set; }
    
    [Column("IS_SUCCESS")]
    public bool IsSuccess { get; set; } = true;
    
    [Column("ERROR_MESSAGE")]
    [StringLength(4000)]
    public string? ErrorMessage { get; set; }
    
    [Column("HASH")]
    [StringLength(128)]
    public string? Hash { get; set; }
}