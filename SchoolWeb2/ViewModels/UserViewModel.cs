using System.ComponentModel.DataAnnotations;

namespace SchoolWeb2.ViewModels
{
	public class UserViewModel
	{
		[Required]
		public string? UserName { get; set; }

		[Required]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",ErrorMessage ="Email is not valid")]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
		 

	}
}
