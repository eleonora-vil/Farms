namespace Mock_Project_Net03.Dtos
{
    public class TrainingProgramUnitModel
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }
        public int SyllabusId { get; set; }
        public SyllabusModel Syllabus { get; set; }
    }
}
