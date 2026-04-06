using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Entities;

[Table("AUCTION_SETTLEMENTS")]
public partial class AuctionSettlement : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("AUCTION_ID")]
	public int AuctionId { get; set; }
	
	[Column("WINNER_ID")]
	public int WinnerId { get; set; }
	
	[Column("FINAL_AMOUNT")]
	[Precision(18,2)]
	public decimal FinalAmount { get; set; }
	
	[Column("PAYMENT_ID")]
	public int PaymentId { get; set; }
	
	[Column("PAYMENT_DUE_DATE")]
	public DateTime PaymentDueDate { get; set; }
	
	[Column("IS_PAID")]
	public bool IsPaid { get; set; }
	
	[Column("PAID_DATE")]
	public DateTime PaidDate { get; set; }
	
	[ForeignKey("AuctionId")]
	[InverseProperty(nameof(Auction.AuctionSettlements))]
	public virtual Auction Auction { get; set; } = null!;

}

