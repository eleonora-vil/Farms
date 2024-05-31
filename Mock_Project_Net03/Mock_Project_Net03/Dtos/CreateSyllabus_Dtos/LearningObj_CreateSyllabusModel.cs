namespace Mock_Project_Net03.Dtos.CreateSyllabus_Dtos
{
    public class LearningObj_CreateSyllabusModel
    {
        public int LearningObjId { get; set; }
        public string? Name { get; set; }
        public DateTime TrainningTime { get; set; }
        public bool? Method { get; set; }
        public int? Index { get; set; }
        public string? Status { get; set; }
        public string? DeliveryType { get; set; }
        public int UnitId { get; set; }
        public int? OutputStandardId { get; set; }
        public string? Duration { get; set; }
    }
}
