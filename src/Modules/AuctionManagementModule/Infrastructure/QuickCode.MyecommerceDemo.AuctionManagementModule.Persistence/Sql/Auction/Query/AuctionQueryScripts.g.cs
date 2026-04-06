namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Auction
    {
        public static class Query
        {
            private const string _prefix = "AuctionManagementModule.Auction.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetActiveAuctions => ResourceKey("GetActiveAuctions.g.sql");
            public static string GetScheduledAuctions => ResourceKey("GetScheduledAuctions.g.sql");
            public static string GetAuctionsBySeller => ResourceKey("GetAuctionsBySeller.g.sql");
            public static string GetAuctionsEndingSoon => ResourceKey("GetAuctionsEndingSoon.g.sql");
            public static string GetAuctionsToClose => ResourceKey("GetAuctionsToClose.g.sql");
            public static string GetClosedAuctionsForSettlement => ResourceKey("GetClosedAuctionsForSettlement.g.sql");
        }
    }
}