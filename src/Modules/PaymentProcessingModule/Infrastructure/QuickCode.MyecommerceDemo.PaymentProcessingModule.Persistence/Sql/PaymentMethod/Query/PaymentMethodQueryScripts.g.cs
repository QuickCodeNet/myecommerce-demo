namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class PaymentMethod
    {
        public static class Query
        {
            private const string _prefix = "PaymentProcessingModule.PaymentMethod.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByCustomerId => ResourceKey("GetByCustomerId.g.sql");
            public static string GetDefaultMethod => ResourceKey("GetDefaultMethod.g.sql");
        }
    }
}