using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QuickCode.MyecommerceDemo.Portal.Helpers.Authorization;
using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;

namespace QuickCode.MyecommerceDemo.Portal.Controllers
{
    [Permission("Dashboard")]
    public class HomeController : BaseController
    {
        public HomeController(ITableComboboxSettingsClient tableComboboxSettingsClient, IHttpContextAccessor httpContextAccessor, IMemoryCache cache) : base(tableComboboxSettingsClient, httpContextAccessor, cache)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View("Privacy");
        }
    }
}
