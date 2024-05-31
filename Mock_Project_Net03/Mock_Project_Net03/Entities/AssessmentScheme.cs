using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("AssessmentScheme")]
    public class AssessmentScheme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssessmentSchemeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? AssessmentSchemeName { get; set; }
        public int? PercentMark { get; set; }

        public ICollection<AssessmentScheme_Syllabus> AssessmentScheme_Syllabus { get; set; }
    }
}
