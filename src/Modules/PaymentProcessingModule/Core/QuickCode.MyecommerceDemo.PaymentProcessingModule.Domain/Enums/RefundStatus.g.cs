using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

public enum RefundStatus{
	[Description("Refund has been requested")]
	Requested,
	[Description("Refund approved, pending processing")]
	Approved,
	[Description("Refund processed by gateway")]
	Processed,
	[Description("Refund request was rejected")]
	Rejected
}
