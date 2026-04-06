using QuickCode.MyecommerceDemo.Common.Nswag.Clients.AuctionManagementModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.AuctionManagementModule
{
    public class BidIncrementRuleData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public BidIncrementRuleDto SelectedItem { get; set; }
        public List<BidIncrementRuleDto> List { get; set; }
    }

    public static partial class BidIncrementRuleExtensions
    {
        public static string GetKey(this BidIncrementRuleDto dto)
        {
            return $"{dto.Id}";
        }
    }
}