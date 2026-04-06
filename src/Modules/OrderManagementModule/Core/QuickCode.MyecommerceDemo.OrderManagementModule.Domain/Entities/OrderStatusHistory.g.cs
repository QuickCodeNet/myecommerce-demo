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

[Table("ORDER_STATUS_HISTORIES")]
public partial class OrderStatusHistory : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("ORDER_ID")]
	public int OrderId { get; set; }
	
	[Column("PREVIOUS_STATUS", TypeName = "nvarchar(250)")]
	public OrderStatus PreviousStatus { get; set; }
	
	[Column("NEW_STATUS", TypeName = "nvarchar(250)")]
	public OrderStatus NewStatus { get; set; }
	
	[Column("REASON")]
	[StringLength(250)]
	public string Reason { get; set; }
	
	[Column("CHANGED_BY_USER_ID")]
	public int ChangedByUserId { get; set; }
	
	[Column("CHANGED_DATE")]
	public DateTime ChangedDate { get; set; }
	
	[ForeignKey("OrderId")]
	[InverseProperty(nameof(Order.OrderStatusHistories))]
	public virtual Order Order { get; set; } = null!;

}

