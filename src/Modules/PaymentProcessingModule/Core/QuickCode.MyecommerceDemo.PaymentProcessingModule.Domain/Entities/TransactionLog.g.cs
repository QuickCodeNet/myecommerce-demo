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

[Table("TRANSACTION_LOGS")]
public partial class TransactionLog : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public long Id { get; set; }
	
	[Column("PAYMENT_ID")]
	public int PaymentId { get; set; }
	
	[Column("GATEWAY_ID")]
	public int GatewayId { get; set; }
	
	[Column("REQUEST_PAYLOAD")]
	[StringLength(int.MaxValue)]
	public string RequestPayload { get; set; }
	
	[Column("RESPONSE_PAYLOAD")]
	[StringLength(int.MaxValue)]
	public string ResponsePayload { get; set; }
	
	[Column("LOG_LEVEL")]
	[StringLength(50)]
	public string LogLevel { get; set; }
	
	[Column("MESSAGE")]
	[StringLength(1000)]
	public string Message { get; set; }
	
	[Column("TIMESTAMP")]
	public DateTime Timestamp { get; set; }
	
	[ForeignKey("PaymentId")]
	[InverseProperty(nameof(Payment.TransactionLogs))]
	public virtual Payment Payment { get; set; } = null!;


	[ForeignKey("GatewayId")]
	[InverseProperty(nameof(PaymentGateway.TransactionLogs))]
	public virtual PaymentGateway PaymentGateway { get; set; } = null!;

}

