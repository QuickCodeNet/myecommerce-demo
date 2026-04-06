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

[Table("BID_INCREMENT_RULES")]
public partial class BidIncrementRule : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("PRICE_FROM")]
	[Precision(18,2)]
	public decimal PriceFrom { get; set; }
	
	[Column("PRICE_TO")]
	[Precision(18,2)]
	public decimal PriceTo { get; set; }
	
	[Column("INCREMENT_AMOUNT")]
	[Precision(18,2)]
	public decimal IncrementAmount { get; set; }
	
	[Column("IS_ACTIVE")]
	public bool IsActive { get; set; }
	}

