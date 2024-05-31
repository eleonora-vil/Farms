using Mock_Project_Net03.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse
{
    public class GetSyllabusByIdResponse
    {
        public int SyllabusId { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        public string? Code { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? Outline { get; set; }
        public string? Level { get; set; }
        public string? Version { get; set; }

        [MaxLength(65535)]
        public string? TechnicalRequirement { get; set; }
        [MaxLength(65535)]
        public string? CourseObjectives { get; set; }

        [MaxLength(65535)]
        public string? TrainingDelivery { get; set; }

        public string? Status { get; set; }
        public int? AttendeeNumber { get; set; }

        public int? InstructorId { get; set; }
        public string InstructorName { get; set; }
        //public string InstructorLevel { get; set; }

        public int? Slot { get; set; }
       
        public List<TrainingProgramModel>? TrainingProgram { get; set; } = new List<TrainingProgramModel>();
        public List<TrainingProgramUnitResponse>? Unit { get; set; } = new List<TrainingProgramUnitResponse>();
        public List<AssessmentSchemaResponse>? assessmentSchemeSyllabus { get; set; } = new List<AssessmentSchemaResponse>() {
        };

 //       public List<LearningObjResponse>? LearningObj { get; set; } = new List<LearningObjResponse>();

       // public List<MaterialsResponse>? Materials { get; set; } = new List<MaterialsResponse>();
    }
    
}
