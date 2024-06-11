using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWeb2.Models
{
	public class Student
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public DateTime DateOfBirth { get; set; }

		[NotMapped]
		public string FullName { get => $"{ FirstName} { LastName}";   }
	}
}
