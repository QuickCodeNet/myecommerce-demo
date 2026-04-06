namespace QuickCode.MyecommerceDemo.OrderManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class OrderItem
    {
        public static class Query
        {
            private const string _prefix = "OrderManagementModule.OrderItem.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByOrderId => ResourceKey("GetByOrderId.g.sql");
            public static string GetProductSalesCount => ResourceKey("GetProductSalesCount.g.sql");
        }
    }
}