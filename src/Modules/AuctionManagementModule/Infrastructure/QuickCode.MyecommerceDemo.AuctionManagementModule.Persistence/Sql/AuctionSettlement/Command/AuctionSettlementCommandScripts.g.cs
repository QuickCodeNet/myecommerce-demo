namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class AuctionSettlement
    {
        public static class Command
        {
            private const string _prefix = "AuctionManagementModule.AuctionSettlement.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string MarkAsPaid => ResourceKey("MarkAsPaid.g.sql");
        }
    }
}