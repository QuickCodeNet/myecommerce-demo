namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class PortalPageAccessGrant
    {
        public static class Query
        {
            private const string _prefix = "IdentityModule.PortalPageAccessGrant.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetPortalPageAccessGrants => ResourceKey("GetPortalPageAccessGrants.g.sql");
            public static string GetPortalPageAccessGrant => ResourceKey("GetPortalPageAccessGrant.g.sql");
        }
    }
}