using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.IdentityModule
{
    public class RefreshTokenData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public RefreshTokenDto SelectedItem { get; set; }
        public List<RefreshTokenDto> List { get; set; }
    }

    public static partial class RefreshTokenExtensions
    {
        public static string GetKey(this RefreshTokenDto dto)
        {
            return $"{dto.Id}";
        }
    }
}