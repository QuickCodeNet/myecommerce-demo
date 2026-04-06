using Newtonsoft.Json.Linq;

namespace QuickCode.MyecommerceDemo.Common.Models;

public class RequestInfo
{
    public string Path { get; set; } = default!;
    public string Method { get; set; } = default!;
    public Dictionary<string, string> Headers { get; set; } = [];
    
    public JObject Body { get; set; } = default!;
}