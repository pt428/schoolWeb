using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb2.DTO;
using SchoolWeb2.Models;
using SchoolWeb2.Service;

namespace SchoolWeb2.Controllers
{
	[Authorize(Roles = "Admin,Teacher")]
	public class StudentsController : Controller
	{
		private StudentService _studentService;

		public StudentsController(StudentService studentService)
		{
			_studentService = studentService;
		}
		//**************************
		// INDEX READ
		//**************************
		 
		public async Task<IActionResult> IndexAsync([FromQuery] StudentParameters studentParameter)
		{
			var allStudents = await _studentService.GetAllStudentsAsync(studentParameter);
			return View(allStudents);
		}
		//**************************
		// CREATE START
		//**************************
		public IActionResult Create()
		{
			return View();
		}
		//**************************
		// CREATE END
		//**************************
		[HttpPost]
		public async Task<IActionResult> CreateAsync(StudentDTO studentDTO)
		{
			await _studentService.AddStudentAsync(studentDTO);
			return RedirectToAction("Index");
		}

		//**************************
		//  UPDATE START
		//**************************
		public async Task<IActionResult> Update(int id)
		{
			//var studentDTO = new StudentDTO();
			var student = await _studentService.GetStudentByIDAsync(id);
			if (student != null)
			{
				return View(student);
			}
			return View("NotFound");
		}
		//**************************
		// UPDATE END
		//**************************
		[HttpPost]
		public async Task<IActionResult> Update(StudentDTO studentDTO)
		{
			await _studentService.UpdateStudentAsync(studentDTO);

			return RedirectToAction("Index");
		}
		//**************************
		// DELETE
		//**************************
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var studentDTO = await _studentService.GetStudentByIDAsync(id);
			if (studentDTO != null)
			{
				await _studentService.DeleteStudentAsync(id);

				return RedirectToAction("Index");
			}

			return View("NotFound");
		}

	}
}
