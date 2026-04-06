using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.IdentityModule
{
    public class PortalPageDefinitionData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public PortalPageDefinitionDto SelectedItem { get; set; }
        public List<PortalPageDefinitionDto> List { get; set; }
    }

    public static partial class PortalPageDefinitionExtensions
    {
        public static string GetKey(this PortalPageDefinitionDto dto)
        {
            return $"{dto.Key}";
        }
    }
}