using Microsoft.AspNetCore.Identity;

namespace FreshShopMaster.Models
{	//linked with dbo AspNetUser
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

	

	}
}
