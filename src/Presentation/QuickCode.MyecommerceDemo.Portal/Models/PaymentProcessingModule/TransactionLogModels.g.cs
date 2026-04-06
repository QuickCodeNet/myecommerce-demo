using QuickCode.MyecommerceDemo.Common.Nswag.Clients.PaymentProcessingModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.PaymentProcessingModule
{
    public class TransactionLogData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public TransactionLogDto SelectedItem { get; set; }
        public List<TransactionLogDto> List { get; set; }
    }

    public static partial class TransactionLogExtensions
    {
        public static string GetKey(this TransactionLogDto dto)
        {
            return $"{dto.Id}";
        }
    }
}