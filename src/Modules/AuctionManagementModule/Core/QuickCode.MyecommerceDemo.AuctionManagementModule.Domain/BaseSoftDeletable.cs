using System;
using System.ComponentModel.DataAnnotations.Schema;
using QuickCode.MyecommerceDemo.Common;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Domain;

public class BaseSoftDeletable : ISoftDeletable
{
    [Column("IsDeleted")]
    public bool IsDeleted { get; set; }
    
    [Column("DeletedOnUtc")]
    public DateTime? DeletedOnUtc { get; set; }
}