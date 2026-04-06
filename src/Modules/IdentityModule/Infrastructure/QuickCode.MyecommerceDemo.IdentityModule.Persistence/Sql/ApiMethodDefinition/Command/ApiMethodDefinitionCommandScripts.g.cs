namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class ApiMethodDefinition
    {
        public static class Command
        {
            private const string _prefix = "IdentityModule.ApiMethodDefinition.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string DeleteApiMethodDefinitionsWithModuleName => ResourceKey("DeleteApiMethodDefinitionsWithModuleName.g.sql");
            public static string DeleteApiMethodDefinitionsWithModelName => ResourceKey("DeleteApiMethodDefinitionsWithModelName.g.sql");
        }
    }
}