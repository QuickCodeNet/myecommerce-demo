using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.IdentityModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Domain.Entities;

[PrimaryKey("PermissionGroupName", "PortalPageDefinitionKey", "PageAction")]
[Table("PortalPageAccessGrants")]
public partial class PortalPageAccessGrant : IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("PermissionGroupName")]
	[StringLength(1000)]
	public string PermissionGroupName { get; set; }
	
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("PortalPageDefinitionKey")]
	[StringLength(1000)]
	public string PortalPageDefinitionKey { get; set; }
	
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("PageAction", TypeName = "nvarchar(250)")]
	public PageActionType PageAction { get; set; }
	
	[Column("ModifiedBy", TypeName = "nvarchar(250)")]
	public ModificationType ModifiedBy { get; set; }
	
	[Column("IsActive")]
	public bool IsActive { get; set; }
	
	[ForeignKey("PortalPageDefinitionKey")]
	[InverseProperty(nameof(PortalPageDefinition.PortalPageAccessGrants))]
	public virtual PortalPageDefinition PortalPageDefinition { get; set; } = null!;


	[ForeignKey("PermissionGroupName")]
	[InverseProperty(nameof(PermissionGroup.PortalPageAccessGrants))]
	public virtual PermissionGroup PermissionGroup { get; set; } = null!;

}

