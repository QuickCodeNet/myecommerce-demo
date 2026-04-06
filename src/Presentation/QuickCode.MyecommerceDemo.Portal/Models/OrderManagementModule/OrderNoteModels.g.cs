using QuickCode.MyecommerceDemo.Common.Nswag.Clients.OrderManagementModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.OrderManagementModule
{
    public class OrderNoteData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public OrderNoteDto SelectedItem { get; set; }
        public List<OrderNoteDto> List { get; set; }
    }

    public static partial class OrderNoteExtensions
    {
        public static string GetKey(this OrderNoteDto dto)
        {
            return $"{dto.Id}";
        }
    }
}