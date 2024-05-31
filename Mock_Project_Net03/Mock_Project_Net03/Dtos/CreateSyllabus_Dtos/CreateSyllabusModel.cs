using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Dtos.CreateSyllabus_Dtos
{
    public class CreateSyllabusModel
    {
        public Syllabus_CreateSyllabusModel SyllabusModel { get; set; }
        public List<CreateTrainingProgramUnit> CreateTrainingProgramUnits { get; set; }
        public List<AssessmentScheme_CreateSyllabusModel> AssessmentSchemeSyllabusModels { get; set; }

    }


    public class CreateTrainingProgramUnit
    {
        public TrainingProgramUnit_CreateSyllabusModel TrainingProgramUnitModel { get; set; }
        public List<CreateLearningObject> CreateLearningObjects { get; set; }
    }

    public class CreateLearningObject
    {
        public LearningObj_CreateSyllabusModel LearningObjModel { get; set; }
        public List<MaterialModel>? MaterialModels { get; set; }
    }
}

