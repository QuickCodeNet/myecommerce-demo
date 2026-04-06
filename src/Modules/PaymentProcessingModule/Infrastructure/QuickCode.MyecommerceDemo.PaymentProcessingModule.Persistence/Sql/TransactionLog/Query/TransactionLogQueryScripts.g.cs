namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class TransactionLog
    {
        public static class Query
        {
            private const string _prefix = "PaymentProcessingModule.TransactionLog.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByPaymentId => ResourceKey("GetByPaymentId.g.sql");
            public static string SearchLogs => ResourceKey("SearchLogs.g.sql");
        }
    }
}