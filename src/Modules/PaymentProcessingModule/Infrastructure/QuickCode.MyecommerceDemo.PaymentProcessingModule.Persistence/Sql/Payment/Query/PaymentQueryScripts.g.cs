namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Payment
    {
        public static class Query
        {
            private const string _prefix = "PaymentProcessingModule.Payment.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByOrderId => ResourceKey("GetByOrderId.g.sql");
            public static string GetByReferenceId => ResourceKey("GetByReferenceId.g.sql");
            public static string GetByStatus => ResourceKey("GetByStatus.g.sql");
            public static string GetFailedPayments => ResourceKey("GetFailedPayments.g.sql");
            public static string GetPaymentsToCapture => ResourceKey("GetPaymentsToCapture.g.sql");
            public static string GetPaymentWithGateway => ResourceKey("GetPaymentWithGateway.g.sql");
            public static string GetDailyVolume => ResourceKey("GetDailyVolume.g.sql");
        }
    }
}