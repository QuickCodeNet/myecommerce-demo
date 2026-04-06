using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.IdentityModule
{
    public class ModelData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public ModelDto SelectedItem { get; set; }
        public List<ModelDto> List { get; set; }
    }

    public static partial class ModelExtensions
    {
        public static string GetKey(this ModelDto dto)
        {
            return $"{dto.Name}|{dto.ModuleName}";
        }
    }
}