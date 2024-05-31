using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Dtos
{
    public class Syllabus_CreateSyllabusModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SyllabusId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; }

        public string? Outline { get; set; }
        public string? Level { get; set; }
        public string? Version { get; set; }

        public string? TechnicalRequirement { get; set; }

        public string? CourseObjectives { get; set; }

        public string? TrainingDelivery { get; set; }

        public string Status { get; set; }

        public int? AttendeeNumber { get; set; }
        public int InstructorId { get; set; }
        public int? Slot { get; set; }
       
    }
}
