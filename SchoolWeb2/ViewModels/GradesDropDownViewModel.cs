using SchoolWeb2.Models;

namespace SchoolWeb2.ViewModels
{
	public class GradesDropDownViewModel
	{
		public List<Student> Students { get; set; }
		public List<Subject> Subjects { get; set; }
        public GradesDropDownViewModel()
        {
            Students=new List<Student>();
            Subjects=new List<Subject>();
        }
    }
}
