using Microsoft.IdentityModel.Tokens;
using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Requests;

public class UpdateClassRequest
{
    public string? ClassName { get; set; }
    public string? ClassCode { get; set; }
    public int? ProgramId { get; set; }

    public int? SemesterId { get; set; }

    public int? InstructorId { get; set; }
}

public static class UpdateClassRequestExtension
{
    public static ClassModel ToClassModel(this UpdateClassRequest req)
    {
        return new ClassModel()
        {
            ClassName = req.ClassName,
            ClassCode = req.ClassCode,
            ProgramId = req.ProgramId,
            SemesterId = req.SemesterId,
            InstructorId = req.InstructorId
        };
    }
}