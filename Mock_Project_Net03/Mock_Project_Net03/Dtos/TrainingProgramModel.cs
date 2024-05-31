using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos
{
    public class TrainingProgramModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgramId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProgramName { get; set; }

        public string Description { get; set; }

        public string CreateBy { get; set; }

        public string? LastUpdatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string? Version { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public ICollection<TrainingProgram_Syllabus>? TrainingProgram_Syllabus { get; set; }

    }

}