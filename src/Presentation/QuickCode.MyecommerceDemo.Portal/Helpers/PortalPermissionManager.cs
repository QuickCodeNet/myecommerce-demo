using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCode.MyecommerceDemo.Common.Nswag;
using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using QuickCode.MyecommerceDemo.Portal.Controllers;

namespace QuickCode.MyecommerceDemo.Portal.Helpers
{
    public interface IPortalPagePermissionManager
    {
        Task<ViewPermission> GetPagePermission(string currentController, string viewName);
    }

    public class PortalPagePermissionManager : IPortalPagePermissionManager
    {
        protected IHttpContextAccessor httpContextAccessor;
        private IPortalPageDefinitionsClient portalPageDefinitionsClient;
        private IPortalPageAccessGrantsClient portalPageAccessGrantClient;

        public IHttpContextAccessor GetHttpContextAccessor()
        {
            if (httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                if (httpContextAccessor.HttpContext!.User.Claims.Any(i => i.Type.Equals("QuickCodeApiToken")))
                {
                    var claimAuthToken =
                        httpContextAccessor.HttpContext!.User.Claims.First(i => i.Type.Equals("QuickCodeApiToken"));
       
    
                    ((ClientBase)portalPageDefinitionsClient).SetBearerToken(claimAuthToken!.Value);
                    ((ClientBase)portalPageAccessGrantClient).SetBearerToken(claimAuthToken!.Value);
                }
            }
            return httpContextAccessor;
        }

         
        public PortalPagePermissionManager(IPortalPageDefinitionsClient portalPageDefinitionsClient,
            IPortalPageAccessGrantsClient portalPageAccessGrantClient,
            IHttpContextAccessor httpContextAccessor)
        {
            this.portalPageDefinitionsClient = portalPageDefinitionsClient;
            this.portalPageAccessGrantClient = portalPageAccessGrantClient;
            this.httpContextAccessor = httpContextAccessor;
        }


        private async Task<IEnumerable<SelectListItem>> GetSessionSelectListItem(string key)
        {
            IEnumerable<SelectListItem> returnValue = new List<SelectListItem>();
            if ((returnValue = GetHttpContextAccessor().HttpContext.Session.Get<IEnumerable<SelectListItem>>(key)) == null)
            {
                returnValue = key switch
                {
                    "PortalPagePermissionTypes" => Enum.GetNames<PageActionType>().AsMultiSelectList(),
                    "PortalPageDefinitions" => (await portalPageDefinitionsClient.PortalPageDefinitionsListAsync())
                        .AsMultiSelectList("Key", "{0}{1}", "ModuleName", "ModelName"),
                    _ => returnValue
                };

                GetHttpContextAccessor().HttpContext.Session.Set(key, returnValue);
            }

            return returnValue;
        }


        private async Task<List<PortalPageAccessGrantDto>> GetPortalPageAccessGrant()
        {
            var key = "PortalPageAccessGrants";
            List<PortalPageAccessGrantDto> returnValue;
            if ((returnValue =
                    GetHttpContextAccessor().HttpContext!.Session.Get<List<PortalPageAccessGrantDto>>(key)) !=
                null) 
                return returnValue;
            
            var portalPagePermissionAccess = await portalPageAccessGrantClient.PortalPageAccessGrantsListAsync();
            
            returnValue =
                portalPagePermissionAccess.Where(i =>
                    i.IsActive &&
                    i.PermissionGroupName == SessionGroupName).ToList<PortalPageAccessGrantDto>();
            GetHttpContextAccessor().HttpContext!.Session.Set(key, returnValue);

            return returnValue;
        }


        public async Task<ViewPermission> GetPagePermission(string currentController, string viewName)
        {
            var authorization = new ViewPermission();
            var portalPageDefinitions= await GetSessionSelectListItem("PortalPageDefinitions");
            var portalPermissionResultList = from A in portalPageDefinitions
                where A.Text == currentController
                select A.Value;
            

            var authorizationsGroupInfo = await GetPortalPageAccessGrant();
            var result = authorizationsGroupInfo
                .Where(i => portalPermissionResultList.Contains(i.PortalPageDefinitionKey));

            var portalPageAccessGrantDtos = Enumerable.ToList(result);
            if (portalPageAccessGrantDtos.Any())
            {
                authorization.IsPageAvailable = true;
                foreach (var item in portalPageAccessGrantDtos)
                {
                    string typeName = viewName;
                    var portalPagePermissionTypes = await GetSessionSelectListItem("PortalPagePermissionTypes");
                    var authorizationsList = (from A in portalPagePermissionTypes
                        where A.Text.Equals(item.PageAction.ToString())
                        select A.Text);

                    foreach (var authType in authorizationsList)
                    {
                        authorization.GetType().GetProperty(authType)!.SetValue(authorization, true, null);
                    }
                }
            }

            return authorization;
        }


        public string SessionGroupName
        {
            get
            {
                var groupName = GetHttpContextAccessor().HttpContext.User.Claims.Where(i => i.Type == ClaimTypes.GroupSid).FirstOrDefault().Value;
                return groupName;
            }
        }
    }
}
