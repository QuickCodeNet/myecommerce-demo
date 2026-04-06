namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class PaymentGateway
    {
        public static class Query
        {
            private const string _prefix = "PaymentProcessingModule.PaymentGateway.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetActive => ResourceKey("GetActive.g.sql");
        }
    }
}