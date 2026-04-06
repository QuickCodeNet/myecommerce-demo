using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.IdentityModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;

namespace QuickCode.MyecommerceDemo.IdentityModule.Domain.Entities;

[Table("PortalMenus")]
public partial class PortalMenu : IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("Key")]
	[StringLength(1000)]
	public string Key { get; set; }
	
	[Column("Name")]
	[StringLength(250)]
	public string Name { get; set; }
	
	[Column("Text")]
	[StringLength(250)]
	public string Text { get; set; }
	
	[Column("Tooltip")]
	[StringLength(250)]
	public string Tooltip { get; set; }
	
	[Column("ActionName")]
	[StringLength(250)]
	public string ActionName { get; set; }
	
	[Column("OrderNo")]
	public int OrderNo { get; set; }
	
	[Column("ParentName")]
	[StringLength(250)]
	public string ParentName { get; set; }
	}

