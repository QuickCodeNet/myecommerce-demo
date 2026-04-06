using Newtonsoft.Json.Linq;

namespace QuickCode.MyecommerceDemo.Common.Workflows;

public class Step
{
    public string Url { get; set; } = null!;
    public string Method { get; set; } = null!;
    public Dictionary<string, string> Headers { get; set; } = [];
    public JObject Body { get; set; } = null!;
    public int? TimeoutSeconds { get; set; } =  null!;
    public List<ConditionalAction> Transitions { get; set; } = [];
    public Dictionary<string, Step> Steps { get; set; } = [];
    public List<string> DependsOn { get; set; } = [];
    public string Condition { get; set; } = null!;
    public string Repeat { get; set; } = null!;
}