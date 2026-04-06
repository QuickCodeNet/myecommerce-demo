using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Entities;

[Table("AUCTIONS")]
public partial class Auction : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("ITEM_ID")]
	public int ItemId { get; set; }
	
	[Column("ITEM_NAME")]
	[StringLength(250)]
	public string ItemName { get; set; }
	
	[Column("ITEM_DESCRIPTION")]
	[StringLength(1000)]
	public string ItemDescription { get; set; }
	
	[Column("SELLER_ID")]
	public int SellerId { get; set; }
	
	[Column("START_PRICE")]
	[Precision(18,2)]
	public decimal StartPrice { get; set; }
	
	[Column("RESERVE_PRICE")]
	[Precision(18,2)]
	public decimal ReservePrice { get; set; }
	
	[Column("CURRENT_PRICE")]
	[Precision(18,2)]
	public decimal CurrentPrice { get; set; }
	
	[Column("WINNING_BID_ID")]
	public int WinningBidId { get; set; }
	
	[Column("WINNER_ID")]
	public int WinnerId { get; set; }
	
	[Column("START_TIME")]
	public DateTime StartTime { get; set; }
	
	[Column("END_TIME")]
	public DateTime EndTime { get; set; }
	
	[Column("STATUS", TypeName = "nvarchar(250)")]
	public AuctionStatus Status { get; set; }
	
	[Column("CREATED_DATE")]
	[StringLength(50)]
	public string CreatedDate { get; set; }
	
	[InverseProperty(nameof(Bid.Auction))]
	public virtual ICollection<Bid> Bids { get; } = new List<Bid>();


	[InverseProperty(nameof(AuctionWatchlist.Auction))]
	public virtual ICollection<AuctionWatchlist> AuctionWatchlists { get; } = new List<AuctionWatchlist>();


	[InverseProperty(nameof(AuctionSettlement.Auction))]
	public virtual ICollection<AuctionSettlement> AuctionSettlements { get; } = new List<AuctionSettlement>();


	[ForeignKey("ItemId")]
	[InverseProperty(nameof(AuctionItem.Auctions))]
	public virtual AuctionItem AuctionItem { get; set; } = null!;


	[ForeignKey("WinningBidId")]
	[InverseProperty(nameof(Bid.Auctions))]
	public virtual Bid Bid { get; set; } = null!;

}

