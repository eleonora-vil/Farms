using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Common.Payloads.Responses;

public class GetAllSemesterResponse
{
    public List<Semester> Semesters { get; set; }
}