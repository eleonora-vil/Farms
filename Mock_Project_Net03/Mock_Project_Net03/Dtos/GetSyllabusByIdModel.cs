using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos
{
    public class GetSyllabusByIdModel
    {
        public int SyllabusId { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Outline { get; set; }

        public string? Level { get; set; }

        public string? Version { get; set; }

        [MaxLength(65535)]
        public string? TechnicalRequirement { get; set; }

        [MaxLength(65535)]
        public string? CourseObjectives { get; set; }

        [MaxLength(65535)]
        public string? TrainingDelivery { get; set; }

        public string? Status { get; set; }

        public int? AttendeeNumber { get; set; }

        public int? InstructorId { get; set; }

        public string InstructorName { get; set; }
        public string InstructorLevel { get; set;}
    }
}
