using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

public enum PageActionType{
	[Description("List page")]
	List,
	[Description("Details / view page")]
	Detail,
	[Description("Create page")]
	Insert,
	[Description("Update page")]
	Update,
	[Description("Delete page")]
	Delete
}
