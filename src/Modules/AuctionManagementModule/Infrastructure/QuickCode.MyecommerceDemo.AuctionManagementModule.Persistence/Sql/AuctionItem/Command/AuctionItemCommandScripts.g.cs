namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class AuctionItem
    {
        public static class Command
        {
            private const string _prefix = "AuctionManagementModule.AuctionItem.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string MarkAsUnavailable => ResourceKey("MarkAsUnavailable.g.sql");
        }
    }
}