using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class GetTrainingProgramDetails
    {
        public string ProgramName { get; set; }

        public string Description { get; set; }

        public string CreateBy { get; set; }
        public string Status { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public List<SyllabusModel> ListSyllabus { get; set; }
    }
}
