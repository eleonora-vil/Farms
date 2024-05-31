using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Dtos
{
    public class ManaModel
    {
        public int totalSyllabus { get; set; }
        public int totalTrainingProgram { get; set; }
        public int totalClass { get; set; }
        public int totalUser { get; set; }
        public int totalRoleSuperAdmin { get; set; }
        public int totalRoleAdmin { get; set; }
        public int totalRoleTrainer { get; set; }
        public int totalRoleTrainee { get; set; }
        public List<SyllabusModel> listSyllabus { get; set;}

    }
}
