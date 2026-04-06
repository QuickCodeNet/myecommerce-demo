using QuickCode.MyecommerceDemo.Common.Nswag.Clients.OrderManagementModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.OrderManagementModule
{
    public class ShippingMethodData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public ShippingMethodDto SelectedItem { get; set; }
        public List<ShippingMethodDto> List { get; set; }
    }

    public static partial class ShippingMethodExtensions
    {
        public static string GetKey(this ShippingMethodDto dto)
        {
            return $"{dto.Id}";
        }
    }
}