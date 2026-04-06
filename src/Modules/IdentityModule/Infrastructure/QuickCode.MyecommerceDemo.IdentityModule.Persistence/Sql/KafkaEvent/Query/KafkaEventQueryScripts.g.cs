namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class KafkaEvent
    {
        public static class Query
        {
            private const string _prefix = "IdentityModule.KafkaEvent.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetKafkaEvents => ResourceKey("GetKafkaEvents.g.sql");
            public static string GetActiveKafkaEvents => ResourceKey("GetActiveKafkaEvents.g.sql");
            public static string GetTopicWorkflows => ResourceKey("GetTopicWorkflows.g.sql");
            public static string GetTopicWorkflowsKafkaEvents => ResourceKey("GetTopicWorkflowsKafkaEvents.g.sql");
        }
    }
}