namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Brand
    {
        public static class Command
        {
            private const string _prefix = "ProductCatalogModule.Brand.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string Deactivate => ResourceKey("Deactivate.g.sql");
        }
    }
}