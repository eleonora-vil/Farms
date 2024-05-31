using Microsoft.IdentityModel.Tokens;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;

namespace Mock_Project_Net03.Services;

public class SemesterService
{
    private readonly IRepository<Semester, int> _semesterRepo;
    public SemesterService(IRepository<Semester,int> semesterRepo)
    {
        _semesterRepo = semesterRepo;
    }

    public List<Semester> GetAllSemester()
    {
        var semesters = _semesterRepo.GetAll().ToList();
        if (semesters.IsNullOrEmpty())
        {
            throw new BadRequestException("There is no semester");
        }

        return semesters;
    }

    public async Task<Semester> GetSemesterById(int id)
    {
        var semester = await _semesterRepo.GetByIdAsync(id);
        if (semester is null)
        {
            throw new BadRequestException("This semester is not existed");
        }

        return semester;
    }
}