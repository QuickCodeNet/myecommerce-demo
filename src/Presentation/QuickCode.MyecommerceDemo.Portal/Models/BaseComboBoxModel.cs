using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCode.MyecommerceDemo.Portal.Helpers;

namespace QuickCode.MyecommerceDemo.Portal.Models
{
    public class BaseComboBoxModel
    {
        public Dictionary<string, Dictionary<string, string>> ComboList { get; set; } = new();

        public string GetComboBoxValue(string comboKey, object itemId)
        {
            if (itemId == null || !ComboList.ContainsKey(comboKey))
                return "[null]";

            var itemIdString = itemId.ToString();
            return ComboList[comboKey].TryGetValue(itemIdString!, out var value) ? value : "[null]";
        }
        
        public IEnumerable<SelectListItem> GetComboBoxList(string comboKey, object value)
        {
            return ComboList[comboKey].Select(item => new SelectListItem()
                { Selected = (value.AsString() == item.Key), Value = item.Key, Text = item.Value }).ToList();
        }

    }
}