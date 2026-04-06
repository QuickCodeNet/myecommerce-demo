using QuickCode.MyecommerceDemo.Common.Nswag.Clients.PaymentProcessingModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.PaymentProcessingModule
{
    public class RefundData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public RefundDto SelectedItem { get; set; }
        public List<RefundDto> List { get; set; }
    }

    public static partial class RefundExtensions
    {
        public static string GetKey(this RefundDto dto)
        {
            return $"{dto.Id}";
        }
    }
}