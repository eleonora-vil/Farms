using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("LearningObj")]
    public class LearningObj
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LearningObjId { get; set; }

        public string? Name { get; set; }
        public DateTime? TrainningTime { get; set; }
        public bool? Method { get; set; }
        public int? Index { get; set; }
        public string? Status { get; set; }
        public string? DeliveryType { get; set; }

        public string? Duration { get; set; }


        [ForeignKey("TrainingProgramUnitObj")]
        public int? UnitId { get; set; }
        public virtual TrainingProgramUnit Unit { get; set; }

        [ForeignKey("OutputStandardObj")]
        public int? OutputStandardId { get; set; }
        public virtual OutputStandard OutputStandard { get; set; }
    }
}
