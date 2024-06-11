using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb2.Models;
using SchoolWeb2.ViewModels;

namespace SchoolWeb2.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private UserManager<AppUser> _userManager;
		private SignInManager<AppUser> _signInManager;

		public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		//**************************
		// LOGIN START
		//**************************
		[AllowAnonymous]
		public IActionResult Login(string returnUrl)
		{
			LoginViewModel loginModel = new LoginViewModel();
			loginModel.ReturnUrl = returnUrl;
			return View(loginModel);
		}
		//**************************
		// LOGIN END
		//**************************
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel loginModel)
		{
			if (ModelState.IsValid)
			{
				AppUser? appUser = await _userManager.FindByNameAsync(loginModel.UserName ?? "");
				if (appUser != null)
				{
					await _signInManager.SignOutAsync();
					Microsoft.AspNetCore.Identity.SignInResult result = 
						await _signInManager.PasswordSignInAsync(appUser, loginModel.Password ?? "",loginModel.RememberMe, false);
					if (result.Succeeded)
					{
						return Redirect(loginModel.ReturnUrl ?? "/");
					}
					else
					{
						ModelState.AddModelError(nameof(loginModel.UserName), "Login failed: invalid username or password");

					}
				}
				else
				{

					ModelState.AddModelError(nameof(loginModel.UserName), "Login failed: invalid username or password");
				}
			}
			return View(loginModel);
		}
		//**************************
		// LOGOUT
		//**************************
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
		return RedirectToAction("Index","Home");
		}

		//**************************
		// ACCESS DENIED
		//**************************
		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
