using System;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.ApiMethodDefinition;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Models
{	
    public class PortalPageAccessGrantList
    {
        public string PermissionGroupName { get; set; }
        public List<PortalPagePermissionItem> PortalPageDefinitions { get; set; } = [];
    }

    public class PortalPagePermissionItem
    {
        public string PortalPagePermissionName { get; set; }
        public string ModelName { get; set; }
        public string ModuleName { get; set; }
        public List<PortalPagePermissionTypeItem> PortalPagePermissionTypes { get; set; } = [];
    }

    public class PortalPagePermissionTypeItem
    {
        public PageActionType PortalPagePermissionType { get; set; }
        public bool Value { get; set; }
    }

    public class UpdatePortalPageAccessGrantRequest
    {
        public string PermissionGroupName{ get; set; }
        public string PortalPagePermissionName { get; set; }
        public PageActionType PortalPagePermissionType { get; set; }
        public int Value { get; set; }
    }
	
    public class UpdateApiMethodAccessGrantRequest
    {
        public string PermissionGroupName { get; set; }
        public string ApiMethodDefinitionKey { get; set; }
        public int Value { get; set; }
    }
    
    public record ApiMethodDefinitionItem : ApiMethodDefinitionDto
    {
        public bool Value { get; set; }
    }
	
    public class ApiModulePermissions
    {
        public string PermissionGroupName { get; set; }

        public Dictionary<string, Dictionary<string, List<ApiMethodDefinitionItem>>> ApiModulePermissionList { get; set; } = [];
    }
}