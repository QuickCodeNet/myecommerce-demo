namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class PaymentGateway
    {
        public static class Command
        {
            private const string _prefix = "PaymentProcessingModule.PaymentGateway.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string Deactivate => ResourceKey("Deactivate.g.sql");
        }
    }
}