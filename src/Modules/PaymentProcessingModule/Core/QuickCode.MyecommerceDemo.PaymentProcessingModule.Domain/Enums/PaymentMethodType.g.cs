using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

public enum PaymentMethodType{
	[Description("Credit or Debit Card")]
	CreditCard,
	[Description("PayPal Express Checkout")]
	Paypal,
	[Description("Direct bank transfer")]
	BankTransfer,
	[Description("Apple Pay, Google Pay, etc.")]
	DigitalWallet
}
