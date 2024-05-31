using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class CreateTrainingProgramRequest
    {
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public List<int> SyllabusID { get; set; }
    }
    public static class TrainingProgramExtension
    {
        public static TrainingProgramModel ToTrainingProgram(this CreateTrainingProgramRequest req)
        {
            var trainingModel = new TrainingProgramModel()
            {
                ProgramName = req.ProgramName,
                Description = req.Description,       
            };
            return trainingModel;
        }
       
    }

}
