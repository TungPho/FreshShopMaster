using FreshShopMaster.Helper;
using FreshShopMaster.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreshShopMaster.Components
{
    public class DisplayCheckout : ViewComponent
    {

        public DisplayCheckout()
        {
            
        }
        public IViewComponentResult Invoke()
        {
            return View(HttpContext.Session.GetObjectFromJson<Cart>("cart"));
        }

    }
}
