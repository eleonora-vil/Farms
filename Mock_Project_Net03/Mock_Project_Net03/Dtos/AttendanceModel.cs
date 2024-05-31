using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos
{
    public class AttendanceModel
    {
        public int AttendanceId { get; set; }

        public int ClassId { get; set; }

        public int TraineeId { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [ForeignKey("ClassId")]
        public Class Class { get; set; }

        [ForeignKey("TraineeId")]
        public User Trainee { get; set; }
    }
}
