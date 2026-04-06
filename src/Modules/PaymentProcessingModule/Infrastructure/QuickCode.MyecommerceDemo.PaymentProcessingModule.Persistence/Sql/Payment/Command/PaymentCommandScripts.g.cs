namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Payment
    {
        public static class Command
        {
            private const string _prefix = "PaymentProcessingModule.Payment.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string UpdateStatus => ResourceKey("UpdateStatus.g.sql");
            public static string MarkAsCaptured => ResourceKey("MarkAsCaptured.g.sql");
            public static string MarkAsVoided => ResourceKey("MarkAsVoided.g.sql");
        }
    }
}