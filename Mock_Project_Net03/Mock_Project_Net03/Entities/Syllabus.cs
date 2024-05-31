using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("Syllabus")]
    public class Syllabus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int SyllabusId { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

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
        
        public int InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public User Instructor { get; set; }
        public ICollection<AssessmentScheme_Syllabus> AssessmentScheme_Syllabus { get; set; }
        public ICollection<TrainingProgram_Syllabus > TrainingProgram_Syllabus { get; set; }
    }
}