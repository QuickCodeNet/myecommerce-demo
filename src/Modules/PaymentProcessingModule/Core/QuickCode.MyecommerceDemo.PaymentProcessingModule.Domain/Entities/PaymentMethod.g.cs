using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;

[Table("PAYMENT_METHODS")]
public partial class PaymentMethod : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("CUSTOMER_ID")]
	public int CustomerId { get; set; }
	
	[Column("METHOD_TYPE", TypeName = "nvarchar(250)")]
	public PaymentMethodType MethodType { get; set; }
	
	[Column("TOKEN")]
	[StringLength(250)]
	public string Token { get; set; }
	
	[Column("CARD_BRAND")]
	[StringLength(50)]
	public string CardBrand { get; set; }
	
	[Column("LAST_FOUR_DIGITS")]
	[StringLength(50)]
	public string LastFourDigits { get; set; }
	
	[Column("EXPIRATION_DATE")]
	[StringLength(50)]
	public string ExpirationDate { get; set; }
	
	[Column("IS_DEFAULT")]
	public bool IsDefault { get; set; }
	
	[Column("IS_ACTIVE")]
	public bool IsActive { get; set; }
	}

