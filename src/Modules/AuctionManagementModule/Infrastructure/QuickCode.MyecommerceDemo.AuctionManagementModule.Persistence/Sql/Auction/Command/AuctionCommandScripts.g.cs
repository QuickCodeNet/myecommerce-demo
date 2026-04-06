namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Auction
    {
        public static class Command
        {
            private const string _prefix = "AuctionManagementModule.Auction.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string StartAuction => ResourceKey("StartAuction.g.sql");
            public static string CloseAuction => ResourceKey("CloseAuction.g.sql");
            public static string CancelAuction => ResourceKey("CancelAuction.g.sql");
            public static string SetWinner => ResourceKey("SetWinner.g.sql");
        }
    }
}