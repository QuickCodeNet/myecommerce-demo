namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class ProductAttributeValue
    {
        public static class Command
        {
            private const string _prefix = "ProductCatalogModule.ProductAttributeValue.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string RemoveByProduct => ResourceKey("RemoveByProduct.g.sql");
        }
    }
}