using System.ComponentModel.DataAnnotations.Schema;

namespace QuickCode.MyecommerceDemo.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedOnUtc { get; set; }
}