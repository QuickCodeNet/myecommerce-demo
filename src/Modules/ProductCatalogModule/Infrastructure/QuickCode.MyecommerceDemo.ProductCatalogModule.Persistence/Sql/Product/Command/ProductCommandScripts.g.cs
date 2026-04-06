namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Product
    {
        public static class Command
        {
            private const string _prefix = "ProductCatalogModule.Product.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string UpdatePrice => ResourceKey("UpdatePrice.g.sql");
            public static string AdjustStock => ResourceKey("AdjustStock.g.sql");
            public static string SetStatus => ResourceKey("SetStatus.g.sql");
            public static string Archive => ResourceKey("Archive.g.sql");
        }
    }
}