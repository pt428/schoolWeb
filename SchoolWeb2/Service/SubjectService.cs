using Microsoft.EntityFrameworkCore;
using SchoolWeb2.DTO;
using SchoolWeb2.Models;

namespace SchoolWeb2.Service
{
	public class SubjectService
	{
		private AppDbContext _appDbContext;

		public SubjectService(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		//**************************
		// GET SUBJECT BY ID
		//**************************
		public async Task<SubjectDTO> GetSubjectById(int id)
		{
			var subject = await _appDbContext.Subjects.FirstOrDefaultAsync(x => x.Id == id);
			return ModelToDto(subject);

		}
		//**************************
		// GET ALL SUBJECTS
		//**************************
		public async Task<IEnumerable<SubjectDTO>> GetAllSubjectsAsync()
		{
			var subjectsDTO = new List<SubjectDTO>();
			var subjects = await _appDbContext.Subjects.ToListAsync();

			foreach (var subject in subjects)
			{
				subjectsDTO.Add(ModelToDto(subject));
			}
			return subjectsDTO;
		}
		//**************************
		// ADD SUBJECT
		//**************************
		public async Task AddSubjectAsync(SubjectDTO subjectDTO)
		{
			await _appDbContext.Subjects.AddAsync(DtoToModel(subjectDTO));
			await _appDbContext.SaveChangesAsync();
		}
		//**************************
		// UPDATE SUBJECT
		//**************************
		public async Task UpdateSubjectAsync(SubjectDTO subjectDTO)
		{
			_appDbContext.Subjects.Update(DtoToModel(subjectDTO));
			await _appDbContext.SaveChangesAsync();
		}

		//**************************
		// DELETE SUBJECT
		//**************************
		public async Task DeleteSubjectAsync(int id)
		{
			var subjectToDelete = await _appDbContext.Subjects.FirstOrDefaultAsync(x => x.Id == id);
			_appDbContext.Remove(subjectToDelete);
			await _appDbContext.SaveChangesAsync();
		}
		//**************************
		// DTO TO MODEL
		//**************************
		public Subject DtoToModel(SubjectDTO subjectDTO)
		{
			return new Subject()
			{
				Name = subjectDTO.Name,
				Id = subjectDTO.Id
			};
		}
		//**************************
		// MODEL TO DTO
		//**************************
		public SubjectDTO ModelToDto(Subject subject)
		{
			return new SubjectDTO()
			{
				Name = subject.Name,
				Id = subject.Id
			};
		}
	}

}