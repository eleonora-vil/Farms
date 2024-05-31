using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class TrainingProgramResponse
    {
        public List<TrainingProgramModel> TrainingPrograms { get; set; }
        public string? message { get; set; }

    }
}
