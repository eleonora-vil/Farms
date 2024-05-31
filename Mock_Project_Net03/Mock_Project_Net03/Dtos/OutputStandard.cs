using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Dtos
{
    public class OutputStandardModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OutputStandardId { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
    }
}
