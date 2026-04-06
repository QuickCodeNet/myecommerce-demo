namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class PortalPageDefinition
    {
        public static class Query
        {
            private const string _prefix = "IdentityModule.PortalPageDefinition.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetPortalPageDefinitionsWithModuleName => ResourceKey("GetPortalPageDefinitionsWithModuleName.g.sql");
            public static string GetPortalPageDefinitionsWithModelName => ResourceKey("GetPortalPageDefinitionsWithModelName.g.sql");
            public static string ExistsPortalPageDefinitionsWithModuleName => ResourceKey("ExistsPortalPageDefinitionsWithModuleName.g.sql");
            public static string ExistsPortalPageDefinitionsWithModelName => ResourceKey("ExistsPortalPageDefinitionsWithModelName.g.sql");
        }
    }
}