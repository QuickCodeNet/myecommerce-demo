using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace QuickCode.MyecommerceDemo.Common.Workflows;

public static class WorkflowDeserializer
{
    public static Workflow ParseWorkflow(string yamlContent)
    {
        var builder = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance);
 
        var deserializer = builder
            .WithTypeConverter(new ObjectToJObjectTypeConverter())
            .Build();

        return deserializer.Deserialize<Workflow>(yamlContent);
    }
}