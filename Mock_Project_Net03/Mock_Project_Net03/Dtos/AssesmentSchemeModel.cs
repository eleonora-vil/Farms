using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos
{
    public class AssessmentSchemeModel
    {
        public int AssessmentSchemeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string AssessmentSchemeName { get; set; }
        public int PercentMark { get; set; }

        public ICollection<AssessmentSchemeSyllabusModel> AssessmentSchemeSyllabus { get; set; }
    }
}
