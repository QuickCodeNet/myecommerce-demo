using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.IdentityModule
{
    public class ApiMethodAccessGrantData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public ApiMethodAccessGrantDto SelectedItem { get; set; }
        public List<ApiMethodAccessGrantDto> List { get; set; }
    }

    public static partial class ApiMethodAccessGrantExtensions
    {
        public static string GetKey(this ApiMethodAccessGrantDto dto)
        {
            return $"{dto.PermissionGroupName}|{dto.ApiMethodDefinitionKey}";
        }
    }
}