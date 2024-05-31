using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse
{
    public class AssessmentSchemaResponse
    {


        public int AssessmentSchemeId { get; set; }
        public int SyllabusId { get; set; }
        public int PercentMark { get; set; }
        public string AssessmentSchemeName { get; set; }
        // public AssessmentSchemeSyllabusModel AssessmentScheme { get; set; }
        //public SyllabusModel Syllabus { get; set; }
    }
}

