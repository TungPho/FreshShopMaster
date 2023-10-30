using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;



namespace FreshShopMaster.Controllers
{
	//App roles controller: Create a new Role
	public class AppRolesController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;

        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
		//List all the Roles created by the Admin
        public IActionResult Index()
		{
			var roles = _roleManager.Roles;

			return View(roles);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(IdentityRole model)
		{
			//avoid duplicate role, check if the user exist or not!
			if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
			{
				_roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();

			}
			return RedirectToAction("Index");
		}

	}
}
