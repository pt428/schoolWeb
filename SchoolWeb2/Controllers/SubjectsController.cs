using Microsoft.AspNetCore.Mvc;
using SchoolWeb2.Service;
using SchoolWeb2.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SchoolWeb2.Controllers
{
	[Authorize(Roles = "Admin,Teacher")]
	public class SubjectsController : Controller
	{
		private SubjectService _subjectService;

		public SubjectsController(SubjectService subjectService)
		{
			_subjectService = subjectService;
		}
		//**************************
		// READ INDEX
		//**************************
		public async Task< IActionResult> Index()
		{
			var allSubjects =await _subjectService.GetAllSubjectsAsync();
			return View(allSubjects);
		}
		//**************************
		// ADD START
		//**************************
		public IActionResult Add()
		{
			return View();
		}
		//**************************
		// ADD END
		//**************************
		[HttpPost]
		public async Task<IActionResult> AddAsync(SubjectDTO subjectDTO)
		{
			if (ModelState.IsValid)
			{
				await _subjectService.AddSubjectAsync(subjectDTO);
				return RedirectToAction("Index");
			}
			return View();
		}
		//**************************
		// EDIT START
		//**************************
		public async Task< IActionResult> EditAsync(int id)
		{
			var SubjectToEdit = await _subjectService.GetSubjectById(id);
			return View(SubjectToEdit);
		}
		//**************************
		// EDIT END
		//**************************
		[HttpPost]
		public async Task<IActionResult> EditSubjectAsync(SubjectDTO subjectDTO)
		{

			await _subjectService.UpdateSubjectAsync(subjectDTO);
			return RedirectToAction("Index");
		}
		//**************************
		// DELETE
		//**************************
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await _subjectService.DeleteSubjectAsync(id);
			return RedirectToAction("Index");
		}
	}
}
