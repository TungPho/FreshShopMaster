using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreshShopMaster.Data;
using FreshShopMaster.Models;
using FreshShopMaster.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Collections.Generic;

namespace FreshShopMaster.Controllers
{
	public class ProductsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private int PageSize = 9;


		public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}
		//du lieu chung ta nhan duoc, ep vao FilterData filter
		//To force Web API to read a simple type from the request body, add the [FromBody]

		public class PriceRange
		{
			public int Min { get; set; }
			public int Max { get; set; }

		}
		public IActionResult GetFilteredProducts([FromBody] FilterData filter)
		{
			var filteredProducts = _context.Products.ToList();

			if (filter.PriceRange != null && filter.PriceRange.Count > 0 && !filter.PriceRange.Contains("all-price"))
			{
				List<PriceRange> priceRanges = new List<PriceRange>();
				foreach (var range in filter.PriceRange)
				{
					var value = range.Split("-").ToArray();
					PriceRange priceRange = new PriceRange();
					priceRange.Min = int.Parse(value[0]);
					priceRange.Max = int.Parse(value[1]);
					priceRanges.Add(priceRange);
				}
				filteredProducts = filteredProducts.Where(p => priceRanges.Any(r => p.ProductPrice >= r.Min && p.ProductPrice <= r.Max)).ToList();
			}
			//You surpose to return a HTMl template when the request come!
			return PartialView("_FilterPartialView", filteredProducts); //response khi người dùng gửi request lên server
		}




		// Low-Price -> High-Price
		public async Task<IActionResult> SortByHighPrice()
		{
			return PartialView("_SortedProductsPartial", new ProductListViewModel()
			{
				Products = _context.Products.OrderByDescending(prod => prod.ProductPrice),
				PaingInfo = new PagingInfo()
				{
					ItemsPerPage = PageSize,
					PageIndex = 1,
					TotalItem = _context.Products.Count(),
				}

			});
		}
		// High-Price -> Low-Price
		public async Task<IActionResult> SortByLowPrice()
		{
			return PartialView("_SortedProductsPartial", new ProductListViewModel()
			{
				Products = _context.Products.OrderBy(prod => prod.ProductPrice),
				PaingInfo = new PagingInfo()
				{
					ItemsPerPage = PageSize,
					PageIndex = 1,
					TotalItem = _context.Products.Count(),
				}

			});
		}


		//Product By Cat function
		public IActionResult ProductByCat(int categoryId)
		{
			IEnumerable<Product> products = _context.Products.Where(prod => prod.CategoryId == categoryId);
			return View(products);
		}
		//Search function
		public async Task<IActionResult> Search(string keyword, int page = 1)
		{

			return View("Index",
				new ProductListViewModel()
				{
					Products = _context.Products.Where(prod => prod.ProductName.Contains(keyword)).Skip((int)((page - 1) * PageSize)).Take(PageSize),
					PaingInfo = new PagingInfo()
					{
						ItemsPerPage = PageSize,
						PageIndex = (int)page,
						TotalItem = _context.Products.Count(),
					}
				}
				);
		}

		public IActionResult ConvertToVND(int? page = 1)
		{
			return PartialView("_PartialConvertToVND",
				new ProductListViewModel()
				{

					Products = _context.Products.Skip((int)((page - 1) * PageSize)).Take(PageSize),
					PaingInfo = new PagingInfo()
					{
						ItemsPerPage = PageSize,
						PageIndex = (int)page,
						TotalItem = _context.Products.Count(),
					}

				});
		}




		[Authorize(Roles = "Admin")]
		public IActionResult AdminIndexPage()
		{
			IEnumerable<Product> products = _context.Products.ToList();

			return View(products);
		}




		// GET: Products/?page=1
		public async Task<IActionResult> Index(int? page = 1)
		{
			if (page == 1)
			{
				return _context.Products != null ?
						View("Index",
						   new ProductListViewModel()
						   {

							   Products = _context.Products.Skip((int)((page - 1) * PageSize)).Take(PageSize),
							   PaingInfo = new PagingInfo()
							   {
								   ItemsPerPage = PageSize,
								   PageIndex = (int)page,
								   TotalItem = _context.Products.Count(),
							   }

						   }

							) :
						  Problem("Entity set 'ApplicationDbContext.Products'  is null.");
			}
			else
			{
				return _context.Products != null ?
						PartialView("_PagingPartial",
						   new ProductListViewModel()
						   {

							   Products = _context.Products.Skip((int)((page - 1) * PageSize)).Take(PageSize),
							   PaingInfo = new PagingInfo()
							   {
								   ItemsPerPage = PageSize,
								   PageIndex = (int)page,
								   TotalItem = _context.Products.Count(),
							   }

						   }

							) :
						  Problem("Entity set 'ApplicationDbContext.Products'  is null.");
			}

		}






		// GET: Products/Details/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Products == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.FirstOrDefaultAsync(m => m.ProductId == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// GET: Products/Create
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			//SelectList Item to pass to Create view
            var categories = addCategoryType();
            var productType = addProductType();

            ViewBag.CategoriesId = categories;
            ViewBag.ProductType = productType;


            return View();
		}

		// POST: Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPhoto,ProductDescription,ProductPrice,ProductDiscount,ProductType,OrderId,CategoryId,Photo")] Product product)
		{
			var valueToClean = ModelState["ProductPhoto"];
			valueToClean.Errors.Clear();

			if (ModelState.IsValid)
			{
				if (product.Photo != null)
				{
					string folder = "/images/";

					string fileName = Path.GetFileNameWithoutExtension(product.Photo.FileName);
					string extension = Path.GetExtension(product.Photo.FileName);
					product.ProductPhoto = folder + fileName + extension;

				}



				_context.Add(product);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			var categories = new List<SelectListItem>();

			foreach (var item in _context.Categories)
			{
				categories.Add(new SelectListItem
				{
					Text = item.CategoryName,
					Value = item.CategoryId.ToString()
				});
			}
			ViewBag.CategoriesId = categories;
			return View(product);
		}
		[Authorize(Roles = "Admin")]
		// GET: Products/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Products == null)
			{
				return NotFound();
			}
            var categories = addProductType();
            var productType = addProductType();

            ViewBag.CategoriesId = categories;
            ViewBag.ProductType = productType;

            var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

		// POST: Products/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductDiscount,ProductType,OrderId,Photo")] Product product)
		{

            
			
			if (id != product.ProductId)
			{
				return NotFound();
			}
            if (ModelState.IsValid)
			{
               

			
                try
				{
					string folder = "/images/";
					string fileName = Path.GetFileNameWithoutExtension(product.Photo.FileName);
					string extension = Path.GetExtension(product.Photo.FileName);
					product.ProductPhoto = folder + fileName + extension;
					_context.Update(product);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductExists(product.ProductId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
               
                return RedirectToAction(nameof(Index));
			}
            var categories = addProductType();
            var productType = addProductType();

            ViewBag.CategoriesId = categories;
            ViewBag.ProductType = productType;
            return View(product);
		}

		// GET: Products/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Products == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.FirstOrDefaultAsync(m => m.ProductId == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			Console.WriteLine(id);
			if (_context.Products == null)
			{
				return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
			}
			var product = await _context.Products.FindAsync(id);
			if (product != null)
			{
				Console.WriteLine("Alo");

				_context.Products.Remove(product);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProductExists(int id)
		{
			return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
		}

		public List<SelectListItem> addProductType()
		{
            //SelectList Item to pass to Create view
            var productType = new List<SelectListItem>();

            productType.Add(new SelectListItem()
            {
                Text = "best-seller",
                Value = "best-seller"
            });
            productType.Add(new SelectListItem()
            {
                Text = "top-featured",
                Value = "top-featured"
            });
            return productType;
        }
		public List<SelectListItem> addCategoryType()
		{
            var categories = new List<SelectListItem>();
            foreach (var item in _context.Categories)
            {
                categories.Add(new SelectListItem
                {
                    Text = item.CategoryName,
                    Value = item.CategoryId.ToString()
                });
            }
			return categories;
        }

	}
}
