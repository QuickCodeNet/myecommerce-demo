using QuickCode.MyecommerceDemo.Common.Nswag.Clients.AuctionManagementModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.AuctionManagementModule
{
    public class AuctionItemData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public AuctionItemDto SelectedItem { get; set; }
        public List<AuctionItemDto> List { get; set; }
    }

    public static partial class AuctionItemExtensions
    {
        public static string GetKey(this AuctionItemDto dto)
        {
            return $"{dto.Id}";
        }
    }
}