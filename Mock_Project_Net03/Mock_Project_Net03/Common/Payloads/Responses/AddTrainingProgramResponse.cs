using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class AddTrainingProgramResponse
    {
        // general information for user to know which class they are adding TP
        public string ClassName { get; set; }

        public int ProgramId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Time { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
        
        // content 
        public Class_TrainingUnitModel? ClassTrainingUnit { get; set; }
    }
}
