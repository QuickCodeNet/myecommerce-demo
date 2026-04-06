namespace QuickCode.MyecommerceDemo.Common.Workflows;

public class Workflow
{
    public string Name { get; set; } = null!;
    public string Version { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Dictionary<string, VariableDefinition> Variables { get; set; } = [];
    public Dictionary<string, Step> Steps { get; set; } = [];
}