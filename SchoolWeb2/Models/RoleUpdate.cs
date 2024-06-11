using Microsoft.AspNetCore.Identity;

namespace SchoolWeb2.Models
{
	public class RoleUpdate
	{
		public IdentityRole Role { get; set; }
		public IEnumerable<AppUser> Members { get; set;} 
		public IEnumerable<AppUser> NonMembers { get; set;}
	}
}
