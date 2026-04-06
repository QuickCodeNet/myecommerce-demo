namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class BidIncrementRule
    {
        public static class Query
        {
            private const string _prefix = "AuctionManagementModule.BidIncrementRule.Query";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string GetActiveRules => ResourceKey("GetActiveRules.g.sql");
            public static string GetIncrementForPrice => ResourceKey("GetIncrementForPrice.g.sql");
        }
    }
}