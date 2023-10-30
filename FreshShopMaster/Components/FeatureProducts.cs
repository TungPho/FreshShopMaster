using FreshShopMaster.Data;
using FreshShopMaster.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreshShopMaster.Components
{
    public class FeatureProducts : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public FeatureProducts(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            IEnumerable<Product> products = _context.Products; 
            return View(products);
        }

    }
}
