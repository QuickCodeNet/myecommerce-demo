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

[Table("BIDS")]
public partial class Bid : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("AUCTION_ID")]
	public int AuctionId { get; set; }
	
	[Column("BIDDER_ID")]
	public int BidderId { get; set; }
	
	[Column("BID_AMOUNT")]
	[Precision(18,2)]
	public decimal BidAmount { get; set; }
	
	[Column("BID_TIME")]
	public DateTime BidTime { get; set; }
	
	[Column("STATUS", TypeName = "nvarchar(250)")]
	public BidStatus Status { get; set; }
	
	[InverseProperty(nameof(Auction.Bid))]
	public virtual ICollection<Auction> Auctions { get; } = new List<Auction>();


	[ForeignKey("AuctionId")]
	[InverseProperty(nameof(Auction.Bids))]
	public virtual Auction Auction { get; set; } = null!;

}

