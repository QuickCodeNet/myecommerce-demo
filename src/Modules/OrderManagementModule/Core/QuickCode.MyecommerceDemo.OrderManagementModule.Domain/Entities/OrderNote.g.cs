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

[Table("ORDER_NOTES")]
public partial class OrderNote : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("ORDER_ID")]
	public int OrderId { get; set; }
	
	[Column("NOTE")]
	[StringLength(1000)]
	public string Note { get; set; }
	
	[Column("IS_CUSTOMER_VISIBLE")]
	public bool IsCustomerVisible { get; set; }
	
	[Column("CREATED_BY_USER_ID")]
	public int CreatedByUserId { get; set; }
	
	[Column("CREATED_DATE")]
	public DateTime CreatedDate { get; set; }
	
	[ForeignKey("OrderId")]
	[InverseProperty(nameof(Order.OrderNotes))]
	public virtual Order Order { get; set; } = null!;

}

