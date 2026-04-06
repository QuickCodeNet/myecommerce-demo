namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class Bid
    {
        public static class Command
        {
            private const string _prefix = "AuctionManagementModule.Bid.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string MarkAsOutbid => ResourceKey("MarkAsOutbid.g.sql");
            public static string MarkAsWinning => ResourceKey("MarkAsWinning.g.sql");
        }
    }
}