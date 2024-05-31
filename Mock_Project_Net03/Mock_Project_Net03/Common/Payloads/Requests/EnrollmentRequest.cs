using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class EnrollmentRequest
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
