using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb2.DTO;
using SchoolWeb2.Models;
using SchoolWeb2.Service;

namespace SchoolWeb2.Controllers.API
{
	[Route("api/students")]
	[ApiController]
	[Authorize(Roles = "Admin,Teacher")]
	public class StudentsApiController : ControllerBase
	{
		private StudentService _studentService;

		public StudentsApiController(StudentService studentService)
		{
			_studentService = studentService;
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<StudentDTO>>> Index()
		{
			StudentParameters parameters = new StudentParameters() { 
		
			};
			var allStudents = await _studentService.GetAllStudentsAsync(parameters);
			return Ok(allStudents);
		}
	}
}
