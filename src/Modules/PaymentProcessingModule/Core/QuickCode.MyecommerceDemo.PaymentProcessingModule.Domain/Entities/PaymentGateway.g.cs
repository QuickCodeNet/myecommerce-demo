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

[Table("PAYMENT_GATEWAYS")]
public partial class PaymentGateway : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("NAME")]
	[StringLength(50)]
	public string Name { get; set; }
	
	[Column("PROVIDER_CODE")]
	[StringLength(50)]
	public string ProviderCode { get; set; }
	
	[Column("IS_ACTIVE")]
	public bool IsActive { get; set; }
	
	[InverseProperty(nameof(Payment.PaymentGateway))]
	public virtual ICollection<Payment> Payments { get; } = new List<Payment>();


	[InverseProperty(nameof(GatewayConfig.PaymentGateway))]
	public virtual ICollection<GatewayConfig> GatewayConfigs { get; } = new List<GatewayConfig>();


	[InverseProperty(nameof(TransactionLog.PaymentGateway))]
	public virtual ICollection<TransactionLog> TransactionLogs { get; } = new List<TransactionLog>();

}

