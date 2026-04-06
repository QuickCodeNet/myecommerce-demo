namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Category
    {
        public static class Query
        {
            private const string _prefix = "ProductCatalogModule.Category.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetActive => ResourceKey("GetActive.g.sql");
            public static string GetBySlug => ResourceKey("GetBySlug.g.sql");
            public static string GetSubCategories => ResourceKey("GetSubCategories.g.sql");
            public static string GetRootCategories => ResourceKey("GetRootCategories.g.sql");
        }
    }
}