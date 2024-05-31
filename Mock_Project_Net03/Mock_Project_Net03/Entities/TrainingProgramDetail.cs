using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("TrainingProgramDetail")]
    public class TrainingProgramDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }
        public DateTime TrainingDate { get; set; }

        [ForeignKey("TrainingProgram")]
        public int ProgramID { get; set; }
        public virtual TrainingProgram TrainingProgram { get; set; }

        [ForeignKey("TrainingProgramUnit")]
        public int UnitID { get; set; }
        public virtual TrainingProgramUnit Unit { get; set; }
    }
}
