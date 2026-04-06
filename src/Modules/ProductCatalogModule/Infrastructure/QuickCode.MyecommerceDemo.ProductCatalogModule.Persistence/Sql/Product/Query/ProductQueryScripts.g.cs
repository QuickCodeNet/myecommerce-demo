namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Product
    {
        public static class Query
        {
            private const string _prefix = "ProductCatalogModule.Product.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetActive => ResourceKey("GetActive.g.sql");
            public static string GetFeatured => ResourceKey("GetFeatured.g.sql");
            public static string SearchByName => ResourceKey("SearchByName.g.sql");
            public static string GetBySku => ResourceKey("GetBySku.g.sql");
            public static string GetByCategory => ResourceKey("GetByCategory.g.sql");
            public static string GetLowStock => ResourceKey("GetLowStock.g.sql");
            public static string GetProductsWithDetails => ResourceKey("GetProductsWithDetails.g.sql");
            public static string GetRecentlyAdded => ResourceKey("GetRecentlyAdded.g.sql");
        }
    }
}