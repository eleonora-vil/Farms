namespace Mock_Project_Net03.Dtos.CreateSyllabus_Dtos
{
    public class MaterialModel
    {
        public int MaterialsId { get; set; }
        public string Name { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public string? Url { get; set; }
        public int LearningObjId { get; set; }
    }
}
