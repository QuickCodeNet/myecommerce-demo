namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Category
    {
        public static class Command
        {
            private const string _prefix = "ProductCatalogModule.Category.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string Deactivate => ResourceKey("Deactivate.g.sql");
        }
    }
}