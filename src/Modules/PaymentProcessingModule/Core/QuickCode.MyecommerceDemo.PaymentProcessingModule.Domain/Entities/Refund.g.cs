using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;

[Table("REFUNDS")]
public partial class Refund : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("PAYMENT_ID")]
	public int PaymentId { get; set; }
	
	[Column("AMOUNT")]
	[Precision(18,2)]
	public decimal Amount { get; set; }
	
	[Column("REASON")]
	[StringLength(250)]
	public string Reason { get; set; }
	
	[Column("STATUS", TypeName = "nvarchar(250)")]
	public RefundStatus Status { get; set; }
	
	[Column("GATEWAY_REFUND_ID")]
	[StringLength(250)]
	public string GatewayRefundId { get; set; }
	
	[Column("REQUESTED_DATE")]
	public DateTime RequestedDate { get; set; }
	
	[Column("PROCESSED_DATE")]
	public DateTime ProcessedDate { get; set; }
	
	[ForeignKey("PaymentId")]
	[InverseProperty(nameof(Payment.Refunds))]
	public virtual Payment Payment { get; set; } = null!;

}

