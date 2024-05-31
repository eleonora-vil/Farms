using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class AssessmentSchemeResponse
    {
        public AssessmentScheme_ToAdd assessmentSchemeModel { get; set; }
        public List<AssessmentScheme_ToAdd> assessmentSchemes { get; set; }
    }
}
