using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

public enum HttpMethodType{
	[Description("Fetch data")]
	Get,
	[Description("Create or execute")]
	Post,
	[Description("Replace resource")]
	Put,
	[Description("Delete resource")]
	Delete,
	[Description("Partial update")]
	Patch
}
