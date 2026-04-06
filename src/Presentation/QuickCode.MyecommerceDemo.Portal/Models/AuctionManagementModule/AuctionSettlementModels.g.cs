using QuickCode.MyecommerceDemo.Common.Nswag.Clients.AuctionManagementModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.AuctionManagementModule
{
    public class AuctionSettlementData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public AuctionSettlementDto SelectedItem { get; set; }
        public List<AuctionSettlementDto> List { get; set; }
    }

    public static partial class AuctionSettlementExtensions
    {
        public static string GetKey(this AuctionSettlementDto dto)
        {
            return $"{dto.Id}";
        }
    }
}