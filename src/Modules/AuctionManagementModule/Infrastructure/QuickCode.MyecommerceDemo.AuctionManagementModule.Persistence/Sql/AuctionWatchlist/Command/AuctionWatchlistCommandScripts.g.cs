namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class AuctionWatchlist
    {
        public static class Command
        {
            private const string _prefix = "AuctionManagementModule.AuctionWatchlist.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string Unwatch => ResourceKey("Unwatch.g.sql");
        }
    }
}