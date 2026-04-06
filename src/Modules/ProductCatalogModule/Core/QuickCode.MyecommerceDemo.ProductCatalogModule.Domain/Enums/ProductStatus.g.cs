using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

public enum ProductStatus{
	[Description("Product is not yet visible")]
	Draft,
	[Description("Product is visible and available for sale")]
	Active,
	[Description("Product is no longer for sale but kept for records")]
	Archived,
	[Description("Product is visible but not available")]
	OutOfStock
}
