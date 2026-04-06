namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Refund
    {
        public static class Command
        {
            private const string _prefix = "PaymentProcessingModule.Refund.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string Approve => ResourceKey("Approve.g.sql");
            public static string Process => ResourceKey("Process.g.sql");
            public static string Reject => ResourceKey("Reject.g.sql");
        }
    }
}