using Microsoft.AspNetCore.Mvc;
using SchoolWeb2.DTO;
using SchoolWeb2.Service;
using System.Globalization;
using System.Xml;

namespace SchoolWeb2.Controllers
{
	public class FileUploadController : Controller
	{
		private StudentService _studentService;

		public FileUploadController(StudentService studentService)
		{
			_studentService = studentService;
		}
		//**************************
		// UPLOAD FILE
		//**************************
		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile file)
		{
			string filePath = string.Empty;
			if (file.Length > 0)
			{
				filePath = Path.GetFullPath(file.FileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
					stream.Close();
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(filePath);
					XmlElement koren = xmlDocument.DocumentElement;
					foreach (XmlNode node in koren.SelectNodes("/Students/Student"))
					{
						StudentDTO s = new StudentDTO
						{
							FirstName = node.ChildNodes[0].InnerText,
							LastName = node.ChildNodes[1].InnerText,
							DateOfBirth = DateTime.Parse(node.ChildNodes[2].InnerText, CultureInfo.CreateSpecificCulture("cs-CZ"))

						};
						await _studentService.AddStudentAsync(s);
					}
				}
				return RedirectToAction("Index","Students");
			}
			else return View("NotFound");

		}
	}
}
