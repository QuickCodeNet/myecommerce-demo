namespace QuickCode.MyecommerceDemo.OrderManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Order
    {
        public static class Query
        {
            private const string _prefix = "OrderManagementModule.Order.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByCustomerId => ResourceKey("GetByCustomerId.g.sql");
            public static string GetByOrderNumber => ResourceKey("GetByOrderNumber.g.sql");
            public static string GetByStatus => ResourceKey("GetByStatus.g.sql");
            public static string GetRecentOrders => ResourceKey("GetRecentOrders.g.sql");
            public static string GetOrdersForFulfillment => ResourceKey("GetOrdersForFulfillment.g.sql");
            public static string GetOrderWithDetails => ResourceKey("GetOrderWithDetails.g.sql");
            public static string GetMonthlyRevenue => ResourceKey("GetMonthlyRevenue.g.sql");
        }
    }
}