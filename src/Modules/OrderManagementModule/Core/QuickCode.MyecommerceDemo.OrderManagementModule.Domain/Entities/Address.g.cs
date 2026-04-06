using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Entities;

[Table("ADDRESSES")]
public partial class Address : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("CUSTOMER_ID")]
	public int CustomerId { get; set; }
	
	[Column("ADDRESS_LINE_1")]
	[StringLength(250)]
	public string AddressLine1 { get; set; }
	
	[Column("ADDRESS_LINE_2")]
	[StringLength(250)]
	public string AddressLine2 { get; set; }
	
	[Column("CITY")]
	[StringLength(50)]
	public string City { get; set; }
	
	[Column("STATE")]
	[StringLength(50)]
	public string State { get; set; }
	
	[Column("POSTAL_CODE")]
	[StringLength(50)]
	public string PostalCode { get; set; }
	
	[Column("COUNTRY_CODE")]
	[StringLength(50)]
	public string CountryCode { get; set; }
	
	[Column("FULL_ADDRESS")]
	[StringLength(1000)]
	public string FullAddress { get; set; }
	
	[Column("IS_DEFAULT_SHIPPING")]
	public bool IsDefaultShipping { get; set; }
	
	[Column("IS_DEFAULT_BILLING")]
	public bool IsDefaultBilling { get; set; }
	
	[InverseProperty(nameof(Order.ShippingAddress))]
	public virtual ICollection<Order> OrderShippingAddress { get; } = new List<Order>();


	[InverseProperty(nameof(Order.BillingAddress))]
	public virtual ICollection<Order> OrderBillingAddress { get; } = new List<Order>();

}

