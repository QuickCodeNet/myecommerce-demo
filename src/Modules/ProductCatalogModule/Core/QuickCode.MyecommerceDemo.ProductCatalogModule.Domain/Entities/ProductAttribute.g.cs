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

[Table("PRODUCT_ATTRIBUTES")]
public partial class ProductAttribute : BaseSoftDeletable, IAuditableEntity 
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
	
	[InverseProperty(nameof(ProductAttributeValue.ProductAttribute))]
	public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; } = new List<ProductAttributeValue>();

}

