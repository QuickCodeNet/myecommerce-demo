using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickCode.MyecommerceDemo.Portal.Models;

namespace QuickCode.MyecommerceDemo.Portal.ViewComponents
{
    public class UserOperation : ViewComponent
    {
        public UserOperation()
        {

        }

        public IViewComponentResult Invoke()
        {
            var userDetail = new UserDetail();
            if (HttpContext.User.Claims.Where(i => i.Type == ClaimTypes.Name).Count() > 0)
            {
                userDetail.NameSurname = HttpContext.User.Claims.Where(i => i.Type == ClaimTypes.Name).FirstOrDefault().Value;
                userDetail.ImageUrl = "/images/no_image.png";
                
                var groupClaim = HttpContext.User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.GroupSid);
                userDetail.GroupName = groupClaim != null ? groupClaim.Value : "User";
            }
            else
            {
                userDetail.NameSurname = "User";
                userDetail.ImageUrl = "/images/no_image.png";
                userDetail.GroupName = "User";
            }
            return View(userDetail);
        }
    }
}
