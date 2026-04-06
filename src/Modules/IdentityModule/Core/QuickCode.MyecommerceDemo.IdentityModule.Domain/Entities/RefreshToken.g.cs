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

[Table("RefreshTokens")]
public partial class RefreshToken : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("UserId")]
	[StringLength(450)]
	public string UserId { get; set; }
	
	[Column("Token")]
	[StringLength(500)]
	public string Token { get; set; }
	
	[Column("ExpiryDate")]
	public DateTime ExpiryDate { get; set; }
	
	[Column("CreatedDate")]
	public DateTime CreatedDate { get; set; }
	
	[Column("IsRevoked")]
	public bool IsRevoked { get; set; }
	
	[ForeignKey("UserId")]
	[InverseProperty(nameof(AspNetUser.RefreshTokens))]
	public virtual AspNetUser AspNetUser { get; set; } = null!;

}

