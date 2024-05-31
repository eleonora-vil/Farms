using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class GetSyllabusResponse
    {
        public IEnumerable<SyllabusModel> Syllabus { get; set; } = new List<SyllabusModel>();
        public int ToTalPages { get; set; } 
    }
}
