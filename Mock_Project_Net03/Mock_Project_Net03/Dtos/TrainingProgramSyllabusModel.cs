namespace Mock_Project_Net03.Dtos
{
    public class TrainingProgramSyllabusModel
    {
        public int TrainingProgramId { get; set; }
        public int SyllabusId { get; set; }
        public TrainingProgramModel TrainingProgram { get; set; }
        public SyllabusModel Syllabus { get; set; }
    }
}
