using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos
{
    public class ClassModel
    {
        public int ClassId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ClassName { get; set; }

        public int ProgramId { get; set; }

        public int InstructorId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Time { get; set; } // Representing ClassTime enum as string

        [MaxLength(50)]
        public string Status { get; set; }

        public TrainingProgramModel Program { get; set; }

        public UserModel Instructor { get; set; }
    }
}
