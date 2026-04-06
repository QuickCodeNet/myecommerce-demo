using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Auditing;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;

[Table("PRODUCTS")]
public partial class Product : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("SKU")]
	[StringLength(50)]
	public string Sku { get; set; }
	
	[Column("NAME")]
	[StringLength(250)]
	public string Name { get; set; }
	
	[Column("DESCRIPTION")]
	[StringLength(1000)]
	public string Description { get; set; }
	
	[Column("CATEGORY_ID")]
	public int CategoryId { get; set; }
	
	[Column("BRAND_ID")]
	public int BrandId { get; set; }
	
	[Column("PRICE")]
	[Precision(18,2)]
	public decimal Price { get; set; }
	
	[Column("STOCK_QUANTITY")]
	public int StockQuantity { get; set; }
	
	[Column("STATUS", TypeName = "nvarchar(250)")]
	public ProductStatus Status { get; set; }
	
	[Column("IS_FEATURED")]
	public bool IsFeatured { get; set; }
	
	[Column("CREATED_DATE")]
	public DateTime CreatedDate { get; set; }
	
	[InverseProperty(nameof(ProductAttributeValue.Product))]
	public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; } = new List<ProductAttributeValue>();


	[InverseProperty(nameof(ProductReview.Product))]
	public virtual ICollection<ProductReview> ProductReviews { get; } = new List<ProductReview>();


	[ForeignKey("CategoryId")]
	[InverseProperty(nameof(Category.Products))]
	public virtual Category Category { get; set; } = null!;


	[ForeignKey("BrandId")]
	[InverseProperty(nameof(Brand.Products))]
	public virtual Brand Brand { get; set; } = null!;

}

