using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos
{
    public class EnrollmentModel
    {
        public int EnrollmentId { get; set; }

        public int ClassId { get; set; }

        public int TraineeId { get; set; }

        public DateTime EnrollmentDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public int? Grade { get; set; }
    }
}
