using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Dtos
{
    public class ClassModel
    {
        public int ClassId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ClassName { get; set; }

        public string? ClassCode { get; set; }
        public int? ProgramId { get; set; }

        public int? SemesterId { get; set; }

        public int? InstructorId { get; set; }

        public Semester? Semester { get; set; }
        [MaxLength(50)]
        public string? Status { get; set; }

        public TrainingProgram? Program { get; set; }

        public User? Instructor { get; set; }
    }
}
