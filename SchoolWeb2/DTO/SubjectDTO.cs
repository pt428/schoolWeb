using System.ComponentModel.DataAnnotations;

namespace SchoolWeb2.DTO
{
	public class SubjectDTO
	{
		public int Id { get; set; }
		[StringLength(25, ErrorMessage = "The name should not be longer than 25 character")]
		public string Name { get; set; }
	}
}
