using FreshShopMaster.Helper;
using FreshShopMaster.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreshShopMaster.Components
{
	public class SideCart : ViewComponent
	{

		public SideCart()
		{

		}
		public IViewComponentResult Invoke() {
			
			return View(HttpContext.Session.GetObjectFromJson<Cart>("cart")); 
		}
	}
}
