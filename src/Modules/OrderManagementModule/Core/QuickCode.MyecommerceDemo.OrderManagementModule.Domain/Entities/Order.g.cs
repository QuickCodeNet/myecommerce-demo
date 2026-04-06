using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Entities;

[Table("ORDERS")]
public partial class Order : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("ORDER_NUMBER")]
	[StringLength(50)]
	public string OrderNumber { get; set; }
	
	[Column("CUSTOMER_ID")]
	public int CustomerId { get; set; }
	
	[Column("STATUS", TypeName = "nvarchar(250)")]
	public OrderStatus Status { get; set; }
	
	[Column("TOTAL_AMOUNT")]
	[Precision(18,2)]
	public decimal TotalAmount { get; set; }
	
	[Column("SHIPPING_ADDRESS_ID")]
	public int ShippingAddressId { get; set; }
	
	[Column("BILLING_ADDRESS_ID")]
	public int BillingAddressId { get; set; }
	
	[Column("SHIPPING_METHOD_ID")]
	public int ShippingMethodId { get; set; }
	
	[Column("PAYMENT_ID")]
	public int PaymentId { get; set; }
	
	[Column("CREATED_DATE")]
	public DateTime CreatedDate { get; set; }
	
	[Column("UPDATED_DATE")]
	public DateTime UpdatedDate { get; set; }
	
	[InverseProperty(nameof(OrderItem.Order))]
	public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();


	[InverseProperty(nameof(OrderNote.Order))]
	public virtual ICollection<OrderNote> OrderNotes { get; } = new List<OrderNote>();


	[InverseProperty(nameof(OrderStatusHistory.Order))]
	public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; } = new List<OrderStatusHistory>();


	[ForeignKey("ShippingAddressId")]
	[InverseProperty(nameof(Address.OrderShippingAddress))]
	public virtual Address ShippingAddress { get; set; } = null!;


	[ForeignKey("BillingAddressId")]
	[InverseProperty(nameof(Address.OrderBillingAddress))]
	public virtual Address BillingAddress { get; set; } = null!;


	[ForeignKey("ShippingMethodId")]
	[InverseProperty(nameof(ShippingMethod.Orders))]
	public virtual ShippingMethod ShippingMethod { get; set; } = null!;

}

