namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class AuctionItem
    {
        public static class Query
        {
            private const string _prefix = "AuctionManagementModule.AuctionItem.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetAvailableByOwner => ResourceKey("GetAvailableByOwner.g.sql");
        }
    }
}