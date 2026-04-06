using QuickCode.MyecommerceDemo.Common.Nswag.Clients.PaymentProcessingModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.PaymentProcessingModule
{
    public class GatewayConfigData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public GatewayConfigDto SelectedItem { get; set; }
        public List<GatewayConfigDto> List { get; set; }
    }

    public static partial class GatewayConfigExtensions
    {
        public static string GetKey(this GatewayConfigDto dto)
        {
            return $"{dto.Id}";
        }
    }
}