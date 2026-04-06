using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickCode.MyecommerceDemo.Portal.Helpers;
using QuickCode.MyecommerceDemo.Portal.Models;

namespace QuickCode.MyecommerceDemo.Portal.ViewComponents
{
    public class OperationButtons : ViewComponent
    {
        public IPortalPagePermissionManager portalPagePermissionManager { get; set; }
        public OperationButtons(IPortalPagePermissionManager portalPagePermissionManager)
        {
            this.portalPagePermissionManager = portalPagePermissionManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string itemId, string controllerName, string actionName)
        {
            var areaName = ViewContext.RouteData.Values["Area"]!.ToString();
            if (actionName.AsString().Trim().Length == 0)
            {
                actionName = ViewContext.RouteData.Values["Action"]!.ToString();
            }
            if (controllerName.AsString().Trim().Length == 0)
            {
                controllerName = ViewContext.RouteData.Values["Controller"]!.ToString();
            }

            var result = await portalPagePermissionManager.GetPagePermission($"{areaName}{controllerName}", actionName);

            var model = new ViewPermissionItemData
            {
                Item = result,
                ItemId = itemId,
                AreaName = areaName,
                ControllerName = controllerName!.Replace("Controller", string.Empty),
                ActionName = actionName
            };
            
            return View(model);
        }
    }
}
