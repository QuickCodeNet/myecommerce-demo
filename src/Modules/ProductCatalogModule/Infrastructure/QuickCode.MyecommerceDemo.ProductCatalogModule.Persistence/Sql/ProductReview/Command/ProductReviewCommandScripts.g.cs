namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class ProductReview
    {
        public static class Command
        {
            private const string _prefix = "ProductCatalogModule.ProductReview.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string Approve => ResourceKey("Approve.g.sql");
        }
    }
}