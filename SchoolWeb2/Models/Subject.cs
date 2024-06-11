using System.ComponentModel.DataAnnotations;

namespace SchoolWeb2.Models
{
	public class Subject
	{
		public int Id { get; set; }
		//[StringLength(25,ErrorMessage ="The name should not be longer than 25 character")]
		public string? Name { get; set; }
	}
}
