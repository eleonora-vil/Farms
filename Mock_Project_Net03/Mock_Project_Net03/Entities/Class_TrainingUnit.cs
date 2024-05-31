using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("Class_TrainingUnit")]
    public class Class_TrainingUnit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TrainingProgramUnitId { get; set; }
        public int ClassId { get; set; }
        public int TrainerId { get; set; }
        public int RoomId { get; set; }
        public int Slot { get; set; }
        public DateTime Day { get; set; }

        [ForeignKey("TrainingProgramUnitId")]
        public TrainingProgramUnit TrainingProgramUnit { get; set; }

        [ForeignKey("ClassId")]
        public Class Class { get; set; }
 
        [ForeignKey("TrainerId")]
        public User Trainer { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }
    }
}
