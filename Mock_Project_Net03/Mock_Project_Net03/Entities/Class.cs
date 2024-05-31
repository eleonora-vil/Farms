using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    public enum ClassTime
    {
        Morning,
        Afternoon,
        Evening
    }
    [Table("Class")]
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ClassName { get; set; }

        public int ProgramId { get; set; }

        public int InstructorId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ClassTime Time { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [ForeignKey("ProgramId")]
        public TrainingProgram Program { get; set; }

        [ForeignKey("InstructorId")]
        public User Instructor { get; set; }
    }
}
