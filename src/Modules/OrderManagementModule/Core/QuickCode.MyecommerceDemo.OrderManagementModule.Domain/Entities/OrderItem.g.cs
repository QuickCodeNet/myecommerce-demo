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

[Table("ORDER_ITEMS")]
public partial class OrderItem : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("ORDER_ID")]
	public int OrderId { get; set; }
	
	[Column("PRODUCT_ID")]
	public int ProductId { get; set; }
	
	[Column("PRODUCT_NAME")]
	[StringLength(250)]
	public string ProductName { get; set; }
	
	[Column("SKU")]
	[StringLength(50)]
	public string Sku { get; set; }
	
	[Column("QUANTITY")]
	public int Quantity { get; set; }
	
	[Column("UNIT_PRICE")]
	[Precision(18,2)]
	public decimal UnitPrice { get; set; }
	
	[ForeignKey("OrderId")]
	[InverseProperty(nameof(Order.OrderItems))]
	public virtual Order Order { get; set; } = null!;

}

