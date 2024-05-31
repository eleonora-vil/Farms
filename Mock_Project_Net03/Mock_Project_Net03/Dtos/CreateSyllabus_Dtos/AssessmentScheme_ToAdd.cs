using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos.CreateSyllabus_Dtos
{
    public class AssessmentScheme_ToAdd
    {
        public int AssessmentSchemeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string AssessmentSchemeName { get; set; }
        public int PercentMark { get; set; }
    }
}
