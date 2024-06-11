using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb2.Models;
using SchoolWeb2.ViewModels;
using System.ComponentModel;

namespace SchoolWeb2.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private UserManager<AppUser> _userManager;
		private IPasswordHasher<AppUser> _passwordHasher;
		private IPasswordValidator<AppUser> _passwordValidator;

		public UsersController(
			UserManager<AppUser> userManager,
			IPasswordHasher<AppUser> passwordHasher,
			IPasswordValidator<AppUser> passwordValidator)
		{
			_userManager = userManager;
			_passwordHasher = passwordHasher;
			_passwordValidator = passwordValidator;
		}
		//**************************
		// READ INDEX
		//**************************
		public IActionResult Index()
		{
			return View(_userManager.Users);
		}
		//**************************
		// CREATE START
		//**************************
		public ViewResult Create()
		{
			return View();
		}
		//**************************
		// CREATE END
		//**************************
		[HttpPost]
		public async Task<IActionResult> Create(UserViewModel userModel)
		{
			if (ModelState.IsValid)
			{
				AppUser appUser = new AppUser
				{
					UserName = userModel.UserName,
					Email = userModel.Email

				};
				IdentityResult result = await _userManager.CreateAsync(appUser, userModel.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					foreach (IdentityError error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(userModel);
		}
		//**************************
		// UPDATE START
		//**************************
		public async Task<IActionResult> Update(string id)
		{
			var userToUpdate = await _userManager.FindByIdAsync(id);
			if (userToUpdate == null)
			{
				return View("NotFound");
			}
			return View(userToUpdate);
		}
		//**************************
		// UPDATE END
		//**************************
		[HttpPost]
		public async Task<IActionResult> UpdateAsync(string id, string email, string password)
		{
			AppUser? userToUpdate = await _userManager.FindByIdAsync(id);
			if (userToUpdate != null)
			{
				IdentityResult validPass = null;
				if (!string.IsNullOrWhiteSpace(email))
				{
					userToUpdate.Email = email;
				}
				else
				{
					ModelState.AddModelError("", "Email cannot be empty");
				}

				if (!string.IsNullOrWhiteSpace(password))
				{
					validPass = await _passwordValidator.ValidateAsync(_userManager, userToUpdate, password);
					if (validPass.Succeeded)
					{
						userToUpdate.PasswordHash = _passwordHasher.HashPassword(userToUpdate, password);
					}
					else
					{
						Errors(validPass);
					}
				}
				else
				{
					ModelState.AddModelError("", "Password cannot be empty");

				}
				if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
				{
					if (validPass != null && validPass.Succeeded)
					{
						IdentityResult result = await _userManager.UpdateAsync(userToUpdate);
						if (result.Succeeded)
						{
							return RedirectToAction("Index");
						}
						else
						{
							Errors(result);
						}
					}
				}

			}
			else
			{
				ModelState.AddModelError("", "User not found");
			}
			return View(userToUpdate);
		}
		//**************************
		// DELETE
		//**************************
		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			AppUser userToDelete = await _userManager.FindByIdAsync(id);
			if (userToDelete != null)
			{
				IdentityResult result = await _userManager.DeleteAsync(userToDelete);
				if (result.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					Errors(result);
				}
			}
			else
			{
				ModelState.AddModelError("", "User not found");
			}

			return View("Index", _userManager.Users);
		}
		//**************************
		// ADD ERRORS
		//**************************
		private void Errors(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
		}
	}
}
