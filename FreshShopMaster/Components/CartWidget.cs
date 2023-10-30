using FreshShopMaster.Data;
using FreshShopMaster.Helper;
using FreshShopMaster.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreshShopMaster.Components
{
    public class CartWidget : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CartWidget(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(HttpContext.Session.GetObjectFromJson<Cart>("cart"));
        }


    }
}
