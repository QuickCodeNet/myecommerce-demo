namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Sql;
public static partial class SqlScripts
{
    public static partial class PortalMenu
    {
        public static class Command
        {
            private const string _prefix = "IdentityModule.PortalMenu.Command";
            private static string ResourceKey(string sqlName) => $"{_prefix}.{sqlName}";
            public static string DeletePortalMenuItemsWithModuleName => ResourceKey("DeletePortalMenuItemsWithModuleName.g.sql");
            public static string DeletePortalMenuItemsWithModelName => ResourceKey("DeletePortalMenuItemsWithModelName.g.sql");
        }
    }
}