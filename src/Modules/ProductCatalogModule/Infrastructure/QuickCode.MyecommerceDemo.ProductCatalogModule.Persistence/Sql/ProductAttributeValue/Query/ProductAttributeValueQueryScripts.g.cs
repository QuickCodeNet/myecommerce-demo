namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class ProductAttributeValue
    {
        public static class Query
        {
            private const string _prefix = "ProductCatalogModule.ProductAttributeValue.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByProductId => ResourceKey("GetByProductId.g.sql");
            public static string GetAttributesForProduct => ResourceKey("GetAttributesForProduct.g.sql");
        }
    }
}