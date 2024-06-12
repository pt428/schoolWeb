using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWeb2.DTO;
using SchoolWeb2.Models;

namespace SchoolWeb2.Service
{
	public class StudentService
	{
		private AppDbContext _appDbContext;

		public StudentService(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		//**************************
		// GET STUDENT BY ID
		//**************************
		public async Task<StudentDTO> GetStudentByIDAsync(int id)
		{
			var student = await VerifyExistence(id);
			return ModelToDto(student);

		}
		//**************************
		// GET ALL STUDENTS
		//**************************
		public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync(StudentParameters studentParameter)
		{
			IQueryable<Student> allStudents = _appDbContext.Students.OrderBy(x=>x.LastName);
			if (studentParameter.FirstName != null)
			{
				allStudents= allStudents.Where(s => s.FirstName == studentParameter.FirstName);
			}
			if (studentParameter.LastName != null)
			{
				allStudents = allStudents.Where(s => s.LastName == studentParameter.LastName);
			}
		 
			var studentDtos = new List<StudentDTO>();
			foreach (var student in allStudents)
			{
				studentDtos.Add(ModelToDto(student));
			}
			return studentDtos;
		}
		//**************************
		// ADD STUDENT
		//**************************
		public async Task AddStudentAsync(StudentDTO studentDTO)
		{

			await _appDbContext.Students.AddAsync(DtoToModel(studentDTO));
			await _appDbContext.SaveChangesAsync();

		}
		//**************************
		// UPDATE STUDENT
		//**************************
		public async Task UpdateStudentAsync(StudentDTO student)
		{

			_appDbContext.Students.Update(DtoToModel(student));
			await _appDbContext.SaveChangesAsync();
		}

		//**************************
		// DELETE STUDENT
		//**************************
		public async Task DeleteStudentAsync(int id)
		{
			var student = await _appDbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
			_appDbContext.Students.Remove(student);
			await _appDbContext.SaveChangesAsync();
		}
		//**************************
		// MODEL TO DTO
		//**************************
		private StudentDTO ModelToDto(Student student)
		{

			return new StudentDTO()
			{
				Id = student.Id,
				DateOfBirth = student.DateOfBirth,
				FirstName = student.FirstName,
				LastName = student.LastName
			};
		}
		//**************************
		// DTO TO MODEL
		//**************************
		private Student DtoToModel(StudentDTO studentDTO)
		{

			return new Student()
			{
				Id = studentDTO.Id,
				FirstName = studentDTO.FirstName,
				LastName = studentDTO.LastName,
				DateOfBirth = studentDTO.DateOfBirth
			};
		}
		//**************************
		// VERIFY EXISTENCE
		//**************************
		private async Task<Student> VerifyExistence(int id)
		{
			var student = await _appDbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
			if (student == null)
			{
				return null;
			}

			return student;

		}
	}
}
