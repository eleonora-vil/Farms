using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos
{
    public class UpdateSyllabus_AssessmentSchemeModel
    {
        public int AssessmentSchemeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string AssessmentSchemeName { get; set; }
    }
}
