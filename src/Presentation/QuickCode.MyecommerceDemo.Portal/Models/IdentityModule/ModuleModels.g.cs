using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.IdentityModule
{
    public class ModuleData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public ModuleDto SelectedItem { get; set; }
        public List<ModuleDto> List { get; set; }
    }

    public static partial class ModuleExtensions
    {
        public static string GetKey(this ModuleDto dto)
        {
            return $"{dto.Name}";
        }
    }
}