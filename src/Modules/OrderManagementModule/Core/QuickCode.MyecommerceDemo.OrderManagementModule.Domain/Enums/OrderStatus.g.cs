using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

public enum OrderStatus{
	[Description("Order created, awaiting payment")]
	Pending,
	[Description("Payment confirmed, awaiting fulfillment")]
	Paid,
	[Description("Order is being prepared for shipment")]
	Processing,
	[Description("Order has been shipped")]
	Shipped,
	[Description("Order has been delivered")]
	Delivered,
	[Description("Order was cancelled")]
	Cancelled,
	[Description("Order was refunded")]
	Refunded
}
