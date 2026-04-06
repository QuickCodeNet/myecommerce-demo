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

[Table("PAYMENTS")]
public partial class Payment : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("REFERENCE_ID")]
	public Guid ReferenceId { get; set; } = Guid.CreateVersion7();
	
	[Column("ORDER_ID")]
	public int OrderId { get; set; }
	
	[Column("CUSTOMER_ID")]
	public int CustomerId { get; set; }
	
	[Column("AMOUNT")]
	[Precision(18,2)]
	public decimal Amount { get; set; }
	
	[Column("CURRENCY_CODE")]
	[StringLength(50)]
	public string CurrencyCode { get; set; }
	
	[Column("STATUS", TypeName = "nvarchar(250)")]
	public PaymentStatus Status { get; set; }
	
	[Column("PAYMENT_GATEWAY_ID")]
	public int PaymentGatewayId { get; set; }
	
	[Column("GATEWAY_TRANSACTION_ID")]
	[StringLength(250)]
	public string GatewayTransactionId { get; set; }
	
	[Column("CREATED_DATE")]
	public DateTime CreatedDate { get; set; }
	
	[Column("UPDATED_DATE")]
	public DateTime UpdatedDate { get; set; }
	
	[InverseProperty(nameof(Refund.Payment))]
	public virtual ICollection<Refund> Refunds { get; } = new List<Refund>();


	[InverseProperty(nameof(TransactionLog.Payment))]
	public virtual ICollection<TransactionLog> TransactionLogs { get; } = new List<TransactionLog>();


	[ForeignKey("PaymentGatewayId")]
	[InverseProperty(nameof(PaymentGateway.Payments))]
	public virtual PaymentGateway PaymentGateway { get; set; } = null!;

}

