namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class AuctionWatchlist
    {
        public static class Query
        {
            private const string _prefix = "AuctionManagementModule.AuctionWatchlist.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetByUserId => ResourceKey("GetByUserId.g.sql");
            public static string GetWatchersByAuction => ResourceKey("GetWatchersByAuction.g.sql");
            public static string IsWatching => ResourceKey("IsWatching.g.sql");
        }
    }
}