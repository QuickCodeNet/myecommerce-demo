using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

public enum AuctionStatus{
	[Description("Auction is being set up")]
	Draft,
	[Description("Auction is scheduled to start in the future")]
	Scheduled,
	[Description("Auction is currently open for bidding")]
	Active,
	[Description("Bidding has ended, awaiting settlement")]
	Closed,
	[Description("Winner has paid and settlement is complete")]
	Settled,
	[Description("Auction was cancelled before completion")]
	Cancelled
}
