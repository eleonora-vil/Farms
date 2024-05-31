using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Dtos
{
    public class UpdateSyllabusModel
    {
        public string? Name { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public string? Version { get; set; }

        public string TechnicalRequirement { get; set; }

        public string CourseObjectives { get; set; }

        public string TrainingDelivery { get; set; }

        public string Status { get; set; }

        public int AttendeeNumber { get; set; }
        public int InstructorId { get; set; }
        public List<UpdateTrainingProgramUnit> UpdateTrainingProgramUnit { get; set; }
        public List<AssessmentSchemeUpdateSyllabusModel> assessmentSchemeUpdateSyllabusModels { get; set; }
    }

    public class UpdateTrainingProgramUnit
    {
        public TrainingProgramUnitModel trainingProgramUnitModel { get; set; }
        public List<UpdateLearningObject> UpdateLearningObject { get; set; }
    }

    public class UpdateLearningObject
    {
        public LearningObj_CreateSyllabusModel learningObjModel { get; set; }
        public List<MaterialModel> materialModels { get; set; }
        public OutputStandardModel outputStandardModel { get; set; }
    }
}
