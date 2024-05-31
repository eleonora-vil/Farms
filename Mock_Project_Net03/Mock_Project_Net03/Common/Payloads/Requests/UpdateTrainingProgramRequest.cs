using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class UpdateTrainingProgramRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgramId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProgramName { get; set; }

        public string GeneralInformation { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        
        public List<int> SyllabusIds { get; set; }

    }
    public static class UpdateTrainingProgramExtension
    {
        public static TrainingProgramModel ToUpdateTrainingProgram(this UpdateTrainingProgramRequest req)
        {
            var trainingModel = new TrainingProgramModel()
            {
                ProgramId = req.ProgramId,
                ProgramName = req.ProgramName,
                Description = req.GeneralInformation,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                TrainingProgram_Syllabus = new List<TrainingProgram_Syllabus>()
            };
            foreach (var sy in req.SyllabusIds)
            {
                var TrainingProgramSyllabus = new TrainingProgram_Syllabus()
                {
                    TrainingProgramId = req.ProgramId,
                    SyllabusId = sy,
                };
                trainingModel.TrainingProgram_Syllabus.Add(TrainingProgramSyllabus);
            }
            return trainingModel;
        }
    }
}
