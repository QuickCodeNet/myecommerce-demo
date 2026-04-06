using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickCode.MyecommerceDemo.Portal.Models;

namespace QuickCode.MyecommerceDemo.Portal.ViewComponents
{
    public class Pager : ViewComponent
    {

        public Pager()
        {

        }

        public IViewComponentResult Invoke(PagerData pagerData)
        {

            return View(pagerData);
        }

    }


}
