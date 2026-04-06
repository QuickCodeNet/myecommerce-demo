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

[Table("AUCTION_ITEMS")]
public partial class AuctionItem : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("PRODUCT_ID")]
	public int ProductId { get; set; }
	
	[Column("NAME")]
	[StringLength(250)]
	public string Name { get; set; }
	
	[Column("DESCRIPTION")]
	[StringLength(int.MaxValue)]
	public string Description { get; set; }
	
	[Column("CONDITION")]
	[StringLength(50)]
	public string Condition { get; set; }
	
	[Column("OWNER_ID")]
	public int OwnerId { get; set; }
	
	[Column("IS_AVAILABLE")]
	public bool IsAvailable { get; set; }
	
	[InverseProperty(nameof(Auction.AuctionItem))]
	public virtual ICollection<Auction> Auctions { get; } = new List<Auction>();

}

