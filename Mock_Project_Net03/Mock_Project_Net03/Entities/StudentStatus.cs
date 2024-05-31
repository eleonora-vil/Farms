using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("StudentStatus")]
    public class StudentStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        public int EnrollmentId { get; set; }

        [MaxLength(50)]
        public string StatusType { get; set; }

        public string? Note { get; set; }

        public DateTime StatusDate { get; set; }

        [ForeignKey("EnrollmentId")]
        public Enrollment Enrollment { get; set; }
    }
}
