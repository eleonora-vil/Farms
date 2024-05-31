using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("Semester")]
    public class Semester
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SemesterID { get; set; }
        public string SemesterName { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public DateTime SemesterEndDate { get; set; }
    }
}
