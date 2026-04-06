using System;
using System.ComponentModel.DataAnnotations.Schema;
using QuickCode.MyecommerceDemo.Common;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain;

public class BaseSoftDeletable : ISoftDeletable
{
    [Column("IsDeleted")]
    public bool IsDeleted { get; set; }
    
    [Column("DeletedOnUtc")]
    public DateTime? DeletedOnUtc { get; set; }
}