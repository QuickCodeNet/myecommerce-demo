using QuickCode.MyecommerceDemo.Common.Nswag.Clients.PaymentProcessingModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.PaymentProcessingModule
{
    public class PaymentGatewayData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public PaymentGatewayDto SelectedItem { get; set; }
        public List<PaymentGatewayDto> List { get; set; }
    }

    public static partial class PaymentGatewayExtensions
    {
        public static string GetKey(this PaymentGatewayDto dto)
        {
            return $"{dto.Id}";
        }
    }
}