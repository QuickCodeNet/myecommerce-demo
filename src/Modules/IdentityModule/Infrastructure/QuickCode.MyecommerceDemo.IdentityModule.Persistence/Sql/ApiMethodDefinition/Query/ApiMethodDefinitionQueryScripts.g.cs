namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class ApiMethodDefinition
    {
        public static class Query
        {
            private const string _prefix = "IdentityModule.ApiMethodDefinition.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetKafkaEventsApiMethodDefinitions => ResourceKey("GetKafkaEventsApiMethodDefinitions.g.sql");
            public static string GetApiMethodDefinitionsWithModuleName => ResourceKey("GetApiMethodDefinitionsWithModuleName.g.sql");
            public static string GetApiMethodDefinitionsWithModelName => ResourceKey("GetApiMethodDefinitionsWithModelName.g.sql");
            public static string ExistsApiMethodDefinitionsWithModuleName => ResourceKey("ExistsApiMethodDefinitionsWithModuleName.g.sql");
            public static string ExistsApiMethodDefinitionsWithModelName => ResourceKey("ExistsApiMethodDefinitionsWithModelName.g.sql");
        }
    }
}