using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("TrainingProgramUnit")]
    public class TrainingProgramUnit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnitId { get; set; }
        public string? UnitName { get; set; }
        public int? Index { get; set; }
        public int? Time { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public int? SyllabusId { get; set; }

        [ForeignKey("SyllabusId")]
        public virtual Syllabus Syllabus { get; set; }
    }
}
