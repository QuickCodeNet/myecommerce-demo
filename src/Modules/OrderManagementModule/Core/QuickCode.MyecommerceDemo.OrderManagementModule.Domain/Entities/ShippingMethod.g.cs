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

[Table("SHIPPING_METHODS")]
public partial class ShippingMethod : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("NAME")]
	[StringLength(50)]
	public string Name { get; set; }
	
	[Column("DESCRIPTION")]
	[StringLength(250)]
	public string Description { get; set; }
	
	[Column("COST")]
	[Precision(18,2)]
	public decimal Cost { get; set; }
	
	[Column("IS_ACTIVE")]
	public bool IsActive { get; set; }
	
	[InverseProperty(nameof(Order.ShippingMethod))]
	public virtual ICollection<Order> Orders { get; } = new List<Order>();

}

