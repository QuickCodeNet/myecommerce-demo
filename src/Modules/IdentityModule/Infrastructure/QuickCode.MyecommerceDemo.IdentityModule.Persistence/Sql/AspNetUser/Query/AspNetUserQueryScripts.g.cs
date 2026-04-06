namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class AspNetUser
    {
        public static class Query
        {
            private const string _prefix = "IdentityModule.AspNetUser.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetUser => ResourceKey("GetUser.g.sql");
        }
    }
}