namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class PaymentMethod
    {
        public static class Command
        {
            private const string _prefix = "PaymentProcessingModule.PaymentMethod.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string SetDefault => ResourceKey("SetDefault.g.sql");
            public static string Deactivate => ResourceKey("Deactivate.g.sql");
        }
    }
}