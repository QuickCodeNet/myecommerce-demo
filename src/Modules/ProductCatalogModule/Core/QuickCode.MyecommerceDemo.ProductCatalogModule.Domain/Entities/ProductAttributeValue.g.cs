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

[Table("PRODUCT_ATTRIBUTE_VALUES")]
public partial class ProductAttributeValue : BaseSoftDeletable, IAuditableEntity 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("PRODUCT_ID")]
	public int ProductId { get; set; }
	
	[Column("ATTRIBUTE_ID")]
	public int AttributeId { get; set; }
	
	[Column("VALUE")]
	[StringLength(250)]
	public string Value { get; set; }
	
	[ForeignKey("ProductId")]
	[InverseProperty(nameof(Product.ProductAttributeValues))]
	public virtual Product Product { get; set; } = null!;


	[ForeignKey("AttributeId")]
	[InverseProperty(nameof(ProductAttribute.ProductAttributeValues))]
	public virtual ProductAttribute ProductAttribute { get; set; } = null!;

}

