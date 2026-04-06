using QuickCode.MyecommerceDemo.Common.Nswag.Clients.OrderManagementModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.OrderManagementModule
{
    public class AddressData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public AddressDto SelectedItem { get; set; }
        public List<AddressDto> List { get; set; }
    }

    public static partial class AddressExtensions
    {
        public static string GetKey(this AddressDto dto)
        {
            return $"{dto.Id}";
        }
    }
}