using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

public enum PaymentStatus{
	[Description("Payment initiated but not completed")]
	Pending,
	[Description("Payment authorized by the gateway")]
	Authorized,
	[Description("Funds have been captured")]
	Captured,
	[Description("Payment failed")]
	Failed,
	[Description("Authorization has been voided")]
	Voided,
	[Description("Payment has been fully or partially refunded")]
	Refunded
}
