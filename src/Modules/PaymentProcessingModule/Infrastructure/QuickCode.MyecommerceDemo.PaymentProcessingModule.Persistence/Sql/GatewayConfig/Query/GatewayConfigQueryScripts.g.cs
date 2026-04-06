namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class GatewayConfig
    {
        public static class Query
        {
            private const string _prefix = "PaymentProcessingModule.GatewayConfig.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByGatewayId => ResourceKey("GetByGatewayId.g.sql");
        }
    }
}