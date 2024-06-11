using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb2.Models;
using System.Diagnostics;

namespace SchoolWeb2.Controllers
{
	public class HomeController : Controller
	{
		private UserManager<AppUser> _userManager;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
		}

		//**************************
		// INDEX
		//**************************
		[Authorize]
		public async Task<IActionResult> Index()
		{
			AppUser? user = await _userManager.GetUserAsync(HttpContext.User);
			string msg = $" {user.UserName}";
			return View("Index", msg);
		}
		//**************************
		// PRIVACY
		//**************************
		public IActionResult Privacy()
		{
			return View();
		}
		//**************************
		// ERROR
		//**************************
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
