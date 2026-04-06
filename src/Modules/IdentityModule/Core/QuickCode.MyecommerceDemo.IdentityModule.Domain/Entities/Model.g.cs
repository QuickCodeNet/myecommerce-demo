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

[PrimaryKey("Name", "ModuleName")]
[Table("Models")]
public partial class Model : IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("Name")]
	[StringLength(1000)]
	public string Name { get; set; }
	
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("ModuleName")]
	[StringLength(1000)]
	public string ModuleName { get; set; }
	
	[Column("Description")]
	[StringLength(1000)]
	public string Description { get; set; }
	
	[InverseProperty(nameof(ApiMethodDefinition.Model))]
	public virtual ICollection<ApiMethodDefinition> ApiMethodDefinitions { get; } = new List<ApiMethodDefinition>();


	[InverseProperty(nameof(PortalPageDefinition.Model))]
	public virtual ICollection<PortalPageDefinition> PortalPageDefinitions { get; } = new List<PortalPageDefinition>();


	[ForeignKey("ModuleName")]
	[InverseProperty(nameof(Module.Models))]
	public virtual Module Module { get; set; } = null!;

}

