using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolWeb2.DTO;
using SchoolWeb2.Service;
using SchoolWeb2.ViewModels;
using SchoolWeb2.Models;
using Microsoft.AspNetCore.Authorization;

namespace SchoolWeb2.Controllers
{
	[Authorize(Roles = "Admin,Teacher,Student")]
	public class GradesController : Controller
	{
		private GradeService _gradeService;

		public GradesController(GradeService gradeService)
		{
			_gradeService = gradeService;
		}
		//**************************
		// INDEX
		//**************************
		public async Task<IActionResult> Index()
		{
			var allGradesDTO = await _gradeService.GetAllGradesAsync();
			return View(allGradesDTO);
		}
		//**************************
		// CREATE START
		//**************************
		public async Task<IActionResult> Create()
		{
			await FillViewBag();
			return View();
		}
		//**************************
		// CREATE END
		//**************************
		[HttpPost]
		public async Task<IActionResult> CreateAsync(GradeDTO newGradeDTO)
		{
			if (!ModelState.IsValid)
			{
			await	FillViewBag();
				return View(newGradeDTO);
			}
			await _gradeService.AddGrade(newGradeDTO);
			return RedirectToAction("Index");
		}
		//**************************
		// CREATE EDIT START
		//**************************
		public async Task<IActionResult> EditAsync(int id)
		{
			Grade grateToEdit = await _gradeService.GetGradeByIdAsync(id);
			if (grateToEdit == null)
			{
				return View("NotFound");
			}
			GradeDTO grateToEditDto = _gradeService.ModelToDto(grateToEdit);
			await FillViewBag();
				 

			return View(grateToEditDto);
		}
		//**************************
		// CREATE EDIT END
		//**************************
		[HttpPost]
		public async Task<IActionResult> EditAsync(GradeDTO gradeDTO)
		{
			_gradeService.UpdateAsync(gradeDTO);
			return RedirectToAction("Index");
		}
		//**************************
		// CREATE DELETE
		//**************************
		[HttpPost]
		public async Task<IActionResult> DeleteAsync(int id)
		{
		await	_gradeService.DeleteGradeAsync(id);
			return RedirectToAction("Index");
		}

		private async Task FillViewBag()
		{
			var gradesDropDownData = await _gradeService.GetNewGradesDropDownsValues();

			ViewBag.Students = new SelectList(gradesDropDownData.Students, "Id", "FullName");
			ViewBag.Subjects = new SelectList(gradesDropDownData.Subjects, "Id", "Name");
		}
	}
}
