using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class UpdateTrainingProgramResponse
    {
        public string? message { get; set; }
        public string ProgramName { get; set; }

        public string Description { get; set; }

        public string CreateBy { get; set; }
        public string Status { get; set; }

        public List<SyllabusModel> ListSyllabus { get; set; }      
        //public List<int>? SyllabusIds { get; set; }
    }

}
