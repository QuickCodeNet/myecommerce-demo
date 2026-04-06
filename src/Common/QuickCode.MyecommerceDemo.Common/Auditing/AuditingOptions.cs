namespace QuickCode.MyecommerceDemo.Common.Auditing;

public class AuditingOptions
{
    public const string SectionName = "Auditing";
    
    public bool CaptureFullEntityOnUpdate { get; set; } = true;
    
    public bool CaptureFullEntityOnInsert { get; set; } = true;
    
    public bool CaptureFullEntityOnDelete { get; set; } = true;
}

