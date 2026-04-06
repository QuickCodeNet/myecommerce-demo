namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class ApiMethodAccessGrant
    {
        public static class Command
        {
            private const string _prefix = "IdentityModule.ApiMethodAccessGrant.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string ClearApiMethodAccessGrants => ResourceKey("ClearApiMethodAccessGrants.g.sql");
        }
    }
}