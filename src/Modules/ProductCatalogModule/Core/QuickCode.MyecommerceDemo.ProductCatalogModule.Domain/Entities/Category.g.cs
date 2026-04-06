using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;

[Table("CATEGORIES")]
public partial class Category : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("PARENT_CATEGORY_ID")]
	public int ParentCategoryId { get; set; }
	
	[Column("NAME")]
	[StringLength(250)]
	public string Name { get; set; }
	
	[Column("SLUG")]
	[StringLength(50)]
	public string Slug { get; set; }
	
	[Column("IS_ACTIVE")]
	public bool IsActive { get; set; }
	
	[InverseProperty(nameof(Product.Category))]
	public virtual ICollection<Product> Products { get; } = new List<Product>();


	[InverseProperty(nameof(Category.ParentCategory))]
	public virtual ICollection<Category> CategoryParentCategory { get; } = new List<Category>();


	[ForeignKey("ParentCategoryId")]
	[InverseProperty(nameof(Category.CategoryParentCategory))]
	public virtual Category ParentCategory { get; set; } = null!;

}

