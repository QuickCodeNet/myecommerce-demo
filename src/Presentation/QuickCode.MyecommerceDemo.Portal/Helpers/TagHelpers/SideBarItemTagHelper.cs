using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;
using QuickCode.MyecommerceDemo.Common.Nswag;
using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.TagHelpers
{
    [HtmlTargetElement("sidebar-item", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SideBarItemTagHelper : TagHelper
    {
        private readonly IPortalPageDefinitionsClient portalPageDefinitionsClient ;
        private readonly IPortalPageAccessGrantsClient portalPageAccessGrantClient;
        private readonly IPortalMenusClient portalMenusClient;
        private readonly IActionContextAccessor actionContext;
        protected IHttpContextAccessor httpContextAccessor;
        protected readonly IServiceProvider serviceProvider;
        private List<PortalMenuDto> menuItems = [];
        private List<PortalMenuDto> itemList = [];
        private string sideBarIcon = "fa-folder";

        public SideBarItemTagHelper(IActionContextAccessor actionContext, IPortalPageDefinitionsClient portalPageDefinitionsClient ,
            IPortalPageAccessGrantsClient portalPageAccessGrantClient, IPortalMenusClient portalMenusClient, IHttpContextAccessor httpContextAccessor,IServiceProvider serviceProvider)
        {
            this.portalPageAccessGrantClient = portalPageAccessGrantClient;
            this.portalMenusClient = portalMenusClient;
            this.actionContext = actionContext;
            this.portalPageDefinitionsClient = portalPageDefinitionsClient;
            this.httpContextAccessor = httpContextAccessor;
            this.serviceProvider = serviceProvider;

            if (httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                if (httpContextAccessor.HttpContext!.User.Claims.Any(i => i.Type.Equals("QuickCodeApiToken")))
                {
                    var claimAuthToken =
                        httpContextAccessor.HttpContext!.User.Claims.First(i => i.Type.Equals("QuickCodeApiToken"));
                    //httpContextAccessor.HttpContext.Request.Headers.Authorization = $"Bearer {claimAuthToken!.Value}";
                    ((ClientBase)this.portalMenusClient).SetBearerToken(claimAuthToken!.Value);
                    ((ClientBase)this.portalPageDefinitionsClient).SetBearerToken(claimAuthToken!.Value);
                }
            }
        }


        [HtmlAttributeName("title")] public string Title { get; set; }

        [HtmlAttributeName("key")] public string Key { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var groupSid = httpContextAccessor.HttpContext?.User?
                .Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.GroupSid)
                ?.Value;
            if (groupSid != "Admin" && Key.Equals("PermissionManagers"))
            {
                return;
            }
            
            await FillItemLinks();
            
            if (itemList.Count == 0)
            {
                return;
            }

            var contentHtml = GetContentHtml();
            output.TagName = "li";
            output.Attributes.SetAttribute("class", "nav-item");
            output.Content.SetHtmlContent(contentHtml);
        }


        private string GetDivider()
        {
            return $"<div class='collapse-divider'></div>";
        }

        private string GetItemHeader(string headerTitle)
        {
            return $"<h6 class='collapse-header'>{headerTitle}</h6>";
        }

        private string GetItemLink(string linkTitle, string link)
        {
            return $"<a class='collapse-item' href='{link}'>{linkTitle}</a>";
        }

        private List<string> itemLinks = new List<string>();

        private HttpContext GetHttpContext()
        {
            if (actionContext.ActionContext != null)
            {
                return actionContext.ActionContext.HttpContext;
            }
            else
            {
                return httpContextAccessor.HttpContext!;
            }
        }
        
        private async Task FillItemLinks()
        {
            if (menuItems.Count == 0)
            {
                var httpContext = GetHttpContext();
                var mItems = httpContext.Session.Get<List<PortalMenuDto>>("MenuItems");
               
                if (mItems == null)
                {
                    menuItems = (await portalMenusClient.PortalMenusListAsync()).ToList();
                    httpContext.Session.Set("MenuItems", menuItems);
                }
                else
                {
                    menuItems = mItems;
                }
            }

            itemLinks.Clear();

            sideBarIcon = "fa-shield";
            if (Key.Equals("Tables"))
            {
                sideBarIcon = "fa-list";
                itemList = menuItems.Where(i => i.ParentName.AsString().Trim().EndsWith("Module"))
                    .ToList();
            }
            else if (Key.Equals("AuditLogs"))
            {
                sideBarIcon = "fa-shield";
                itemList = menuItems.Where(i => i.ParentName.AsString().Trim().EndsWith("Module"))
                    .ToList();
            }
            else
            {
                if (Key.EndsWith("Settings"))
                {
                    sideBarIcon = "fa-wrench";
                }

                itemList = menuItems.Where(i => i.ParentName.AsString().Trim().Equals(Key))
                    .ToList();
            }

            foreach (var item in itemList)
            {
                item.Text = item.Text.SplitCamelCaseToString();
                item.Tooltip = item.Text.SplitCamelCaseToString();
            }
        }

        private string GetGroupHtml()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in itemLinks)
            {
                sb.AppendLine(item);
            }

            return sb.ToString();
        }

        private string GetCollapseGroupHtml(string groupHtml, bool isCollapsed, string key="")
        {
            StringBuilder sb = new StringBuilder();
            var groupKey = String.IsNullOrEmpty(key) ? Key : key;
            var showCollapse = isCollapsed ? "show" : "";
            sb.AppendLine(
                $"<div id='collapse{groupKey}' class='collapse {showCollapse}' aria-labelledby='heading{groupKey}' data-bs-parent='#accordionSidebar'>");
            sb.AppendLine($"<div class='bg-white py-2 collapse-inner rounded'>");
            sb.AppendLine(groupHtml);
            sb.AppendLine(@"</div>");
            sb.AppendLine(@"</div>");
            return sb.ToString();
        }

        private string GetContentHtml()
        {
            StringBuilder sb = new StringBuilder();
            
            var isCollapsed = false;
            if (Key == "Tables")
            {
                var modules = itemList.Where(i => i.ParentName.EndsWith("Module")).Select(i=>i.ParentName).Distinct()
                    .Distinct();
                foreach (var module in modules)
                {
                    itemList = menuItems.Where(i =>
                            i.ParentName.AsString().Trim().Equals(module) && !i.Name.Equals("AuditLogs"))
                        .ToList();

                    itemLinks.Clear();
                    foreach (var item in itemList)
                    {
                        item.Text = item.Text.SplitCamelCaseToString();
                        item.Tooltip = item.Text.SplitCamelCaseToString();
                    }
                    
                    isCollapsed = false;
                    foreach (var item in itemList.OrderBy(i => i.Text))
                    {
                        var isInTables = menuItems.Where(i =>
                            i.ActionName.Equals(item.ActionName));
                        if (!String.IsNullOrEmpty(item.ActionName) && isInTables.Count() != 2)
                        {
                            itemLinks.Add(GetItemLink(item.Text, item.ActionName));
                            if (!isCollapsed &&
                                httpContextAccessor.HttpContext!.Request.Path.Value!.Equals(item.ActionName))
                            {
                                isCollapsed = true;
                            }
                        }
                    }

                    var groupHtml = GetGroupHtml();
                   
                    var ariaExpanded = isCollapsed ? "true" : "false";
                    sb.AppendLine(
                        $"<a class='nav-link {(isCollapsed ? "" : "collapsed")}' href='#' data-bs-toggle='collapse' data-bs-target='#collapse{module}' aria-expanded='{ariaExpanded}' aria-controls='collapse{module}'>");

                    sb.AppendLine($"<i class='fas fa-fw {sideBarIcon}'></i>");
                    sb.AppendLine($"<span>{ module[..^6].SplitCamelCaseToString()}</span>");
                    sb.AppendLine($"</a>");
                    sb.AppendLine(GetCollapseGroupHtml(groupHtml,isCollapsed, module));
                }
            }
            else if(Key == "AuditLogs")
            {
                var auditLogsItemList = menuItems.Where(i => i.Name.Equals("AuditLogs"))
                    .ToList();
                foreach (var item in auditLogsItemList.OrderBy(i => i.Text))
                {
                    var text = $"{item.ParentName.Replace("Module", "")} {item.Text}";
                    itemLinks.Add(GetItemLink(text.AsSplitCapitalWithUnderline(), item.ActionName));
                    if (!isCollapsed &&
                        httpContextAccessor.HttpContext!.Request.Path.Value!.Equals(item.ActionName))
                    {
                        isCollapsed = true;
                    }
                }
                
                string groupHtml = GetGroupHtml();
                var ariaExpanded = isCollapsed ? "true" : "false";
                sb.AppendLine(
                    $"<a class='nav-link {(isCollapsed ? "" : "collapsed")}' href='#' data-bs-toggle='collapse' data-bs-target='#collapse{Key}' aria-expanded='{ariaExpanded}' aria-controls='collapse{Key}'>");

                sb.AppendLine($"<i class='fas fa-fw {sideBarIcon}'></i>");
                sb.AppendLine($"<span>{Title.SplitCamelCaseToString()}</span>");
                sb.AppendLine($"</a>");
                sb.AppendLine(GetCollapseGroupHtml(groupHtml, isCollapsed));

            }

            else
            {
                foreach (var item in itemList.OrderBy(i => i.Text))
                {
                    itemLinks.Add(GetItemLink(item.Text, item.ActionName));
                    if (!isCollapsed &&
                        httpContextAccessor.HttpContext!.Request.Path.Value!.Equals(item.ActionName))
                    {
                        isCollapsed = true;
                    }
                }
                
                string groupHtml = GetGroupHtml();
                var ariaExpanded = isCollapsed ? "true" : "false";
                sb.AppendLine(
                    $"<a class='nav-link {(isCollapsed ? "" : "collapsed")}' href='#' data-bs-toggle='collapse' data-bs-target='#collapse{Key}' aria-expanded='{ariaExpanded}' aria-controls='collapse{Key}'>");

                sb.AppendLine($"<i class='fas fa-fw {sideBarIcon}'></i>");
                sb.AppendLine($"<span>{Title.SplitCamelCaseToString()}</span>");
                sb.AppendLine($"</a>");
                sb.AppendLine(GetCollapseGroupHtml(groupHtml, isCollapsed));

            }

            return sb.ToString();
        }
    }
}