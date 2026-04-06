namespace QuickCode.MyecommerceDemo.OrderManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Order
    {
        public static class Command
        {
            private const string _prefix = "OrderManagementModule.Order.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string UpdateStatus => ResourceKey("UpdateStatus.g.sql");
            public static string MarkAsPaid => ResourceKey("MarkAsPaid.g.sql");
            public static string MarkAsShipped => ResourceKey("MarkAsShipped.g.sql");
            public static string CancelOrder => ResourceKey("CancelOrder.g.sql");
        }
    }
}