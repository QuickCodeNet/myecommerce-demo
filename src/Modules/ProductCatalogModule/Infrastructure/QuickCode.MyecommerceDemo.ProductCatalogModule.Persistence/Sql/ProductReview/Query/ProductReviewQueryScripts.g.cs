namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class ProductReview
    {
        public static class Query
        {
            private const string _prefix = "ProductCatalogModule.ProductReview.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByProductId => ResourceKey("GetByProductId.g.sql");
            public static string GetPendingApproval => ResourceKey("GetPendingApproval.g.sql");
            public static string GetCustomerReviews => ResourceKey("GetCustomerReviews.g.sql");
            public static string GetAverageRating => ResourceKey("GetAverageRating.g.sql");
        }
    }
}