using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class TrainingProgramByIdResponse
    {
        public TrainingProgramModel trainingProgram {  get; set; }
        public string? message { get; set; }

    }
}
