using System.ComponentModel;

namespace SchoolWeb2.ViewModels
{
	public class GradesViewModel
	{
		public int Id { get; set; }

		[DisplayName("Student name")]
		public string? StudentName { get; set; }

		[DisplayName("Subject name")]
		public string? SubjectName { get; set; }

		public string? What {  get; set; }
		public int Mark {  get; set; }
		public DateTime Date { get; set; }

	}
}
