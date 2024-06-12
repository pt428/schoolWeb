using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb2.DTO;
using SchoolWeb2.Service;

namespace SchoolWeb2.Controllers.API
{
	[Route("api/subjects")]
	[ApiController]
	public class SubjectsApiController : ControllerBase
	{
		private SubjectService _subjectService;

		public SubjectsApiController(SubjectService subjectService)
		{
			_subjectService = subjectService;
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<SubjectDTO>>> Index()
		{
			var allSubject = await _subjectService.GetAllSubjectsAsync();
			return Ok(allSubject);
		}
	}
}
