namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class AuctionSettlement
    {
        public static class Query
        {
            private const string _prefix = "AuctionManagementModule.AuctionSettlement.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByAuctionId => ResourceKey("GetByAuctionId.g.sql");
            public static string GetOutstandingPayments => ResourceKey("GetOutstandingPayments.g.sql");
        }
    }
}