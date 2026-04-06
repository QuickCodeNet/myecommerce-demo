using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models.IdentityModule
{
    public class AspNetUserRoleData : BaseComboBoxModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public AspNetUserRoleDto SelectedItem { get; set; }
        public List<AspNetUserRoleDto> List { get; set; }
    }

    public static partial class AspNetUserRoleExtensions
    {
        public static string GetKey(this AspNetUserRoleDto dto)
        {
            return $"{dto.UserId}|{dto.RoleId}";
        }
    }
}