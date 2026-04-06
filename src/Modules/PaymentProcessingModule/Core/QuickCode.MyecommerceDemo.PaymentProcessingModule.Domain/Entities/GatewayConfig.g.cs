using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;

[Table("GATEWAY_CONFIGS")]
public partial class GatewayConfig : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("GATEWAY_ID")]
	public int GatewayId { get; set; }
	
	[Column("CONFIG_KEY")]
	[StringLength(50)]
	public string ConfigKey { get; set; }
	
	[Column("CONFIG_VALUE")]
	[StringLength(250)]
	public string ConfigValue { get; set; }
	
	[Column("IS_SECRET")]
	public bool IsSecret { get; set; }
	
	[ForeignKey("GatewayId")]
	[InverseProperty(nameof(PaymentGateway.GatewayConfigs))]
	public virtual PaymentGateway PaymentGateway { get; set; } = null!;

}

