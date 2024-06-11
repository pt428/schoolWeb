using SchoolWeb2.Models;
using System.ComponentModel;

namespace SchoolWeb2.DTO
{
	public class GradeDTO
	{
		public int Id { get; set; }
	 
		[DisplayName("Student name")]
		public int StudentId { get; set; }
		[DisplayName("Subject name")]
		public int SubjectId { get; set; }
		public string? What { get; set; }
		public int Mark { get; set; }
		public DateTime Date { get; set; }
	}
}
