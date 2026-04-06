namespace QuickCode.MyecommerceDemo.OrderManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Address
    {
        public static class Query
        {
            private const string _prefix = "OrderManagementModule.Address.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByCustomerId => ResourceKey("GetByCustomerId.g.sql");
            public static string GetDefaultShipping => ResourceKey("GetDefaultShipping.g.sql");
            public static string GetDefaultBilling => ResourceKey("GetDefaultBilling.g.sql");
        }
    }
}