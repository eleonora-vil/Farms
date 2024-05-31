namespace Mock_Project_Net03.Dtos
{
   public class AssessmentSchemeSyllabusModel
    {
        public int AssessmentSchemeId { get; set; }
        public int SyllabusId { get; set; }
        public int PercentMark { get; set; }
        public List< AssessmentSchemeModel> AssessmentScheme { get; set; } = new List< AssessmentSchemeModel>();
        public SyllabusModel Syllabus { get; set; }
    }
}
