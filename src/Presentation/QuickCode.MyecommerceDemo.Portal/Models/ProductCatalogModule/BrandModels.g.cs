using QuickCode.MyecommerceDemo.Common.Nswag.Clients.ProductCatalogModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.ProductCatalogModule
{
    public class BrandData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public BrandDto SelectedItem { get; set; }
        public List<BrandDto> List { get; set; }
    }

    public static partial class BrandExtensions
    {
        public static string GetKey(this BrandDto dto)
        {
            return $"{dto.Id}";
        }
    }
}