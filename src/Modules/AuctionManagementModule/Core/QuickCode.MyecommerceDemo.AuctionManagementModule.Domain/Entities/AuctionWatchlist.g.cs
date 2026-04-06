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

[Table("AUCTION_WATCHLISTS")]
public partial class AuctionWatchlist : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("AUCTION_ID")]
	public int AuctionId { get; set; }
	
	[Column("USER_ID")]
	public int UserId { get; set; }
	
	[ForeignKey("AuctionId")]
	[InverseProperty(nameof(Auction.AuctionWatchlists))]
	public virtual Auction Auction { get; set; } = null!;

}

