using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("TrainingProgram_Syllabus")]
    public class TrainingProgram_Syllabus
    {
        public int TrainingProgramId { get; set; }
        public int SyllabusId { get; set; }
        public string? Status {  get; set; } 
        public TrainingProgram TrainingProgram { get; set; }
        public Syllabus Syllabus { get; set; }
    }
}
