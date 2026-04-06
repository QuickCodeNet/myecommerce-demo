namespace QuickCode.MyecommerceDemo.OrderManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class ShippingMethod
    {
        public static class Query
        {
            private const string _prefix = "OrderManagementModule.ShippingMethod.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetActive => ResourceKey("GetActive.g.sql");
        }
    }
}