namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Bid
    {
        public static class Query
        {
            private const string _prefix = "AuctionManagementModule.Bid.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByAuctionId => ResourceKey("GetByAuctionId.g.sql");
            public static string GetByBidderId => ResourceKey("GetByBidderId.g.sql");
            public static string GetHighestBid => ResourceKey("GetHighestBid.g.sql");
        }
    }
}