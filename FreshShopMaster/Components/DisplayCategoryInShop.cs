using FreshShopMaster.Data;
using FreshShopMaster.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreshShopMaster.Components
{
	public class DisplayCategoryInShop : ViewComponent
	{
        private readonly ApplicationDbContext _context;
        public DisplayCategoryInShop(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            IEnumerable<Category> categories = _context.Categories;
            return View(categories);
        }

    }
}
