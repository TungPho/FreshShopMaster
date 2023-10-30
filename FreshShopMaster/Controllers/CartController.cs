using FreshShopMaster.Data;
using FreshShopMaster.Models;
using Microsoft.AspNetCore.Mvc;
using FreshShopMaster.Helper;

namespace FreshShopMaster.Controllers
{
    public class CartController : Controller
    {   
        public Cart CartModel { get; set; } // an instance on Cart object:
        private readonly ApplicationDbContext _context; //muon su dung du lieu dung ApplicationDbContext
        public CartController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public IActionResult Index()
        {
            return View("Cart", HttpContext.Session.GetObjectFromJson<Cart>("cart"));
        }
        
        
        //Kiem tra co san pham trong co so du lieu khong?
        //Increase product quantity by 1:
        public IActionResult AddToCart(int productId) // truyen vao va lay productId 
        {
            Product? product = _context.Products.FirstOrDefault(prod => prod.ProductId == productId);
            //If the product exist:
            if (product != null)
            {
                //Gan cart = 1 cai session moi ten la cart, neu ma da co thi lay con khong co thi tao ra 1 cart moi
                CartModel = HttpContext.Session.GetObjectFromJson<Cart>("cart") ?? new Cart(); //Convert from the list, by this means: Cart propertie on the top
                CartModel.AddItem(product, 1);//Goi ham AddItem trong class Cart
                HttpContext.Session.SetObjectAsJson("cart", CartModel);
            }
            return View("Cart",CartModel);
        }
       

        //Reduce product quantity by 1:
        public IActionResult ReduceFromCart(int productId) // truyen vao va lay productId 
        {
            Product? product = _context.Products.FirstOrDefault(prod => prod.ProductId == productId);
            //If the product exist:
            if (product != null)
            {
                //Gan cart = 1 cai session moi ten la cart, neu ma da co thi lay con khong co thi tao ra 1 cart moi
                CartModel = HttpContext.Session.GetObjectFromJson<Cart>("cart") ?? new Cart(); //Convert from the list, by this means: Cart propertie on the top
                CartModel.ReduceItem(product);//Goi ham ReduceItem trong class Cart
                HttpContext.Session.SetObjectAsJson("cart", CartModel);
            }
            return View("Cart", CartModel);
        }


        //Remove a line (all item)
        public IActionResult RemoveFromCart(int productId) // truyen vao va lay productId 
        {
            Product? product = _context.Products.FirstOrDefault(prod => prod.ProductId == productId);
            //If the product exist:
            if (product != null)
            {
                //Gan cart = 1 cai session moi ten la cart, neu ma da co thi lay con khong co thi tao ra 1 cart moi
                CartModel = HttpContext.Session.GetObjectFromJson<Cart>("cart"); //Convert from the list, by this means: Cart propertie on the top
                CartModel.RemoveLine(product); //Goi ham RemvoveLine trong cart:
                HttpContext.Session.SetObjectAsJson("cart", CartModel);
            }
            return View("Cart", CartModel);
        }


    }
}
