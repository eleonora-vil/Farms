using Mock_Project_Net03.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class ClassRequest
    {
        public string ClassName { get; set; }

        public int? ProgramId { get; set; }

        public int? InstructorId { get; set; }
        public int? SemesterId { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

    }
    public static class ClassRequestExtensions
    {
        public static ClassModel ToClassModel(this ClassRequest classRequest)
        {
            var classModel = new ClassModel();

            classModel.ClassName = classRequest.ClassName;
            classModel.ProgramId = classRequest.ProgramId;
            classModel.InstructorId = classRequest.InstructorId;
            classModel.SemesterId = classRequest.SemesterId;
            classModel.Status = classRequest.Status;
            
            return classModel;
        }
    }
}
