using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class CreateSyllabusOutlineResponse
    {
        public TrainingProgramUnitModel  Unit { get; set; }
        public string? message { get; set; }
    }
}
