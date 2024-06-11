using Microsoft.EntityFrameworkCore;
using SchoolWeb2.DTO;
using SchoolWeb2.Models;
using SchoolWeb2.ViewModels;

namespace SchoolWeb2.Service
{
	public class GradeService
	{
		private AppDbContext _appDbContext;

		public GradeService(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		//**************************
		// ADD GRADE
		//**************************
		public async Task AddGrade(GradeDTO gradeDTO)
		{
			var grateToAdd = DtoToModel(gradeDTO);
			if (grateToAdd.Student != null && grateToAdd.Subject != null)
			{

				await _appDbContext.AddAsync(grateToAdd);
				await _appDbContext.SaveChangesAsync();
			}
		}
		//**************************
		// UPDATE GRADE
		//**************************
		public async Task UpdateAsync(GradeDTO gradeDTO)
		{
			Grade grade = DtoToModel(gradeDTO);
			_appDbContext.Grades.Update(grade);
			_appDbContext.SaveChanges();
		}
		//**************************
		// GET GRADE BY ID
		//**************************
		public async Task<Grade> GetGradeByIdAsync(int id)
		{
			return await _appDbContext.Grades.Include(x => x.Student).Include(x => x.Subject).FirstOrDefaultAsync(x => x.Id == id);
					}
		//**************************
		// GET ALL GRADES
		//**************************
		public async Task<IEnumerable<GradesViewModel>> GetAllGradesAsync()
		{
			var allGrades = await _appDbContext.Grades.Include(x => x.Student).Include(x => x.Subject).ToListAsync();
			var allGradesDTO = new List<GradesViewModel>();
			foreach (var grade in allGrades)
			{
				allGradesDTO.Add(ModelToViewModel(grade));
			}
			return allGradesDTO;
		}
		//*******************************
		// GET VALUES FOR DROP DOWN LIST
		//*******************************
		public async Task<GradesDropDownViewModel> GetNewGradesDropDownsValues()
		{
			GradesDropDownViewModel gradesDropDownsData = new GradesDropDownViewModel()
			{
				Students = await _appDbContext.Students.OrderBy(x => x.LastName).ToListAsync(),
				Subjects = await _appDbContext.Subjects.OrderBy(x => x.Name).ToListAsync()
			};
			return gradesDropDownsData;
		}
		//**************************
		// DELETE GRADE
		//**************************
		public async Task DeleteGradeAsync(int id)
		{
						var gradeToDelete = await _appDbContext.Grades.FirstOrDefaultAsync(x => x.Id == id);
			_appDbContext.Grades.Remove(gradeToDelete);
			await _appDbContext.SaveChangesAsync();
		}
		//**************************
		// MODEL TO VIEW MODEL
		//**************************
		private GradesViewModel ModelToViewModel(Grade grade)
		{
			return new GradesViewModel
			{
				Id = grade.Id,
				StudentName = grade.Student.FirstName + " " + grade.Student.LastName,
				SubjectName = grade.Subject.Name,
				Mark = grade.Mark,
				What = grade.What,
				Date = grade.Date
			};
		}
		//**************************
		// MODEL TO DTO
		//**************************
		public GradeDTO ModelToDto(Grade grade)
		{
			return new GradeDTO()
			{
				Date = grade.Date,
				StudentId = grade.Student.Id,
				SubjectId = grade.Subject.Id,
				What = grade.What,
				Mark = grade.Mark
			};
		}
		//**************************
		// DTO TO MODEL
		//**************************
		public Grade DtoToModel(GradeDTO gradeDTO)
		{
			return new Grade
			{
				Id = gradeDTO.Id,
				Date = DateTime.Now,
				Student = _appDbContext.Students.FirstOrDefault(x => x.Id == gradeDTO.StudentId),
				Subject = _appDbContext.Subjects.FirstOrDefault(x => x.Id == gradeDTO.SubjectId),
				What = gradeDTO.What,
				Mark = gradeDTO.Mark
			};
		}

	}
}
