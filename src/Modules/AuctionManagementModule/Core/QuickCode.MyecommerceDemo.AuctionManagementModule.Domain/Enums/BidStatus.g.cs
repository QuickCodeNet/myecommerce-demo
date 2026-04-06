using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

public enum BidStatus{
	[Description("The bid is currently a leading bid")]
	Active,
	[Description("The bid has been surpassed by a higher bid")]
	Outbid,
	[Description("The bid that won the auction")]
	Winning,
	[Description("The bid was retracted or cancelled")]
	Cancelled
}
