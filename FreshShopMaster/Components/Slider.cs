using Microsoft.AspNetCore.Mvc;

namespace FreshShopMaster.Components
{
    public class Slider : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
