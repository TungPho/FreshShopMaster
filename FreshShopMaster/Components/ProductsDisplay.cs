using FreshShopMaster.Data;
using FreshShopMaster.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreshShopMaster.Components
{
    public class ProductsDisplay : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ProductsDisplay(ApplicationDbContext context)
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
