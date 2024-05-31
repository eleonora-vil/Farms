using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class SearchTrainingProgramResponse
    {
        public string? message { get; set;}
        public int? page { get; set;}
        public List<TrainingProgramModel>? list { get; set;}
    }
}
