using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using QuickCode.MyecommerceDemo.Portal.Controllers;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.TagHelpers
{
    [HtmlTargetElement(Attributes = nameof(IsPageAvailable))]
    public class IsPageAvailableTagHelper : TagHelper
    {

        public bool IsPageAvailable { get; set; } = false;
        private List<PropertyInfo> viewAuthorizationProperties { get; set; }
        private readonly IActionContextAccessor actionContext;
        public IPortalPagePermissionManager portalPagePermissionManager { get; set; }

        public IsPageAvailableTagHelper(IActionContextAccessor actionContext,
            IPortalPagePermissionManager portalPagePermissionManager)
        {
            this.actionContext = actionContext;
            this.portalPagePermissionManager = portalPagePermissionManager;
            this.viewAuthorizationProperties = typeof(ViewPermission).GetProperties().ToList<PropertyInfo>();
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var operationKeyRemove = "Item";
            var isPermissionManagerPage = false;
            var areaName = actionContext.ActionContext!.RouteData.Values["Area"].AsString();
            var controllerName = actionContext.ActionContext!.RouteData.Values["Controller"].AsString();
            var actionName = actionContext.ActionContext.RouteData.Values["Action"].AsString();
            var operationName = actionName.EndsWith(operationKeyRemove)
                ? actionName.Substring(0, actionName.IndexOf(operationKeyRemove, StringComparison.Ordinal))
                : actionName;
            if (operationName.IsIn("GetModulePermissions", "GetGroupPermissions", "GetKafkaEvents"))
            {
                operationName = "List";
                isPermissionManagerPage = true;
            }

            if (viewAuthorizationProperties.Any(i => i.Name == operationName))
            {
                var result = await portalPagePermissionManager.GetPagePermission($"{areaName}{controllerName}", actionName);

                var actionIsAvailable = result.GetType().GetProperty(operationName)!.GetValue(result).AsBoolean();
                IsPageAvailable = context.AllAttributes[nameof(IsPageAvailable)].Value.AsString().AsBoolean();
                bool isAuthorized;

                if (IsPageAvailable)
                {
                    isAuthorized = actionIsAvailable;
                }
                else
                {
                    isAuthorized = actionIsAvailable == IsPageAvailable;
                }

                if (isPermissionManagerPage
                    && !(result.Insert && result.Delete && result.Update))
                {
                    isAuthorized = false;
                }

                if (!isAuthorized)
                {
                    output.SuppressOutput();
                }

            }

        }

    }
}