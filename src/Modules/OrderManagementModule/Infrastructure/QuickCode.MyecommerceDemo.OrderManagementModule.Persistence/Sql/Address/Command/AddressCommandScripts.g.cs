namespace QuickCode.MyecommerceDemo.OrderManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Address
    {
        public static class Command
        {
            private const string _prefix = "OrderManagementModule.Address.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string SetDefaultShipping => ResourceKey("SetDefaultShipping.g.sql");
        }
    }
}