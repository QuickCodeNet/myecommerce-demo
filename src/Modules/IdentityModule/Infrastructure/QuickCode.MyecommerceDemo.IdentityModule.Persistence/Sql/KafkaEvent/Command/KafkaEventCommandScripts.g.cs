namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class KafkaEvent
    {
        public static class Command
        {
            private const string _prefix = "IdentityModule.KafkaEvent.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string CleanKafkaEventsWithModuleName => ResourceKey("CleanKafkaEventsWithModuleName.g.sql");
            public static string CleanKafkaEventsWithModelName => ResourceKey("CleanKafkaEventsWithModelName.g.sql");
        }
    }
}