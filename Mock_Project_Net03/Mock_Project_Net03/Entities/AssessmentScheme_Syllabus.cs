using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("AssessmentScheme_Syllabus")]
    public class AssessmentScheme_Syllabus
    {
        public int? AssessmentSchemeId { get; set; }
        public int? SyllabusId { get; set; }
        public int? PercentMark { get; set; }
        public AssessmentScheme AssessmentScheme { get; set; }
        public Syllabus Syllabus { get; set; }

    }
}
