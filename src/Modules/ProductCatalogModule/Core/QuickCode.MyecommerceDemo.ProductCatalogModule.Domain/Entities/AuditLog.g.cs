using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;

[Table("AUDIT_LOGS")]
public partial class AuditLog : IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("ID")]
	public Guid Id { get; set; } = Guid.CreateVersion7();
	
	[Column("ENTITY_NAME")]
	[StringLength(250)]
	public string EntityName { get; set; }
	
	[Column("ENTITY_ID")]
	[StringLength(250)]
	public string EntityId { get; set; }
	
	[Column("ACTION")]
	[StringLength(50)]
	public string Action { get; set; }
	
	[Column("USER_ID")]
	[StringLength(250)]
	public string UserId { get; set; }
	
	[Column("USER_NAME")]
	[StringLength(250)]
	public string UserName { get; set; }
	
	[Column("USER_GROUP")]
	[StringLength(250)]
	public string? UserGroup { get; set; }
	
	[Column("TIMESTAMP")]
	public DateTime Timestamp { get; set; }
	
	[Column("OLD_VALUES")]
	[StringLength(int.MaxValue)]
	public string OldValues { get; set; }
	
	[Column("NEW_VALUES")]
	[StringLength(int.MaxValue)]
	public string NewValues { get; set; }
	
	[Column("CHANGED_COLUMNS")]
	[StringLength(int.MaxValue)]
	public string ChangedColumns { get; set; }
	
	[Column("IS_CHANGED")]
	public bool? IsChanged { get; set; }
	
	[Column("CHANGE_SUMMARY")]
	[StringLength(int.MaxValue)]
	public string ChangeSummary { get; set; }
	
	[Column("IP_ADDRESS")]
	[StringLength(50)]
	public string IpAddress { get; set; }
	
	[Column("USER_AGENT")]
	[StringLength(500)]
	public string UserAgent { get; set; }
	
	[Column("CORRELATION_ID")]
	[StringLength(100)]
	public string CorrelationId { get; set; }
	
	[Column("IS_SUCCESS")]
	public bool? IsSuccess { get; set; }
	
	[Column("ERROR_MESSAGE")]
	[StringLength(int.MaxValue)]
	public string ErrorMessage { get; set; }
	
	[Column("HASH")]
	[StringLength(128)]
	public string Hash { get; set; }
	}

