using Mock_Project_Net03.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse
{
    public class MaterialsResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaterialsId { get; set; }
        public string Name { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Url { get; set; }

        [ForeignKey("LearningObjId")]
        public int LearningObjId { get; set; }
        //  public virtual LearningObjModel LearningObj { get; set; }
    }
}
