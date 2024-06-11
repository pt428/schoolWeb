using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb2.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolWeb2.Controllers
{
	[Authorize(Roles = "Admin")]
	public class RolesController : Controller
	{
		private RoleManager<IdentityRole> _roleManager;
		private UserManager<AppUser> _userManager;

		public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}
		//**************************
		// READ INDEX
		//**************************
		public IActionResult Index()
		{
			return View(_roleManager.Roles);
		}
		//**************************
		// CREATE START
		//**************************		
		public IActionResult Create() => View();

		//**************************
		// CREATE START
		//**************************
		[HttpPost]
		public async Task<IActionResult> Create([Required] string name)
		{
			if (ModelState.IsValid)
			{
				IdentityResult identityResult = await _roleManager.CreateAsync(new IdentityRole(name));
				if (identityResult.Succeeded)
				{

					return RedirectToAction("Index");
				}
				AddErrors(identityResult);
			}
			return View(name);
		}
		//**************************
		// UPDATE START
		//**************************		
		public async Task<IActionResult> Update(string id)
		{
			IdentityRole? identityRole = await _roleManager.FindByIdAsync(id);

			List<AppUser> members = new List<AppUser>();
			List<AppUser> nonMembers = new List<AppUser>();

			foreach (AppUser user in _userManager.Users)
			{
				var list = await _userManager.IsInRoleAsync(user, identityRole.Name) ? members : nonMembers;
				list.Add(user);
			}
			return View(new RoleUpdate
			{
				Role = identityRole,
				Members = members,
				NonMembers = nonMembers
			});
		}

		//**************************
		// UPDATE END	
		//**************************
		[HttpPost]
		public async Task<IActionResult> Update(RoleModification roleUpdate)
		{
			IdentityResult identityResult;
			if (ModelState.IsValid)
			{
				foreach (string userId in roleUpdate.AddIds ?? new string[] { })
				{
					AppUser user = await _userManager.FindByIdAsync(userId);
					if (user != null)
					{
						identityResult = await _userManager.AddToRoleAsync(user, roleUpdate.RoleName);
						if (!identityResult.Succeeded)
						{
							AddErrors(identityResult);
						}
					}
				}
				foreach (string userId in roleUpdate.DeleteIds ?? new string[] { })
				{
					AppUser user = await _userManager.FindByIdAsync(userId);
					if (user != null)
					{
						identityResult = await _userManager.RemoveFromRoleAsync(user, roleUpdate.RoleName);
						if (!identityResult.Succeeded)
						{
							AddErrors(identityResult);
						}
					}

				}
					return RedirectToAction("Index");
			}else
				{
					return await Update(roleUpdate.RoleId);
				}
		}
		//**************************
		// DELETE
		//**************************
		[HttpPost]
			public async Task<IActionResult> Delete(string id)
			{
				IdentityRole? identityRole = await _roleManager.FindByIdAsync(id);
				if (identityRole != null)
				{
					IdentityResult identityResult = await _roleManager.DeleteAsync(identityRole);
					if (identityResult.Succeeded)
					{
						return RedirectToAction("Index");
					}
					else
					{
						AddErrors(identityResult);
					}
				}
				else
				{
					ModelState.AddModelError("", "No role found");
				}
				return View("Index", _roleManager.Roles);
			}
			//**************************
			// ADD ERROR TO MODEL STATE
			//**************************
			private void AddErrors(IdentityResult result)
			{
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
		}
	}
