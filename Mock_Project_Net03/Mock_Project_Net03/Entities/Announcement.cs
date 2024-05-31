using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("Announcement")]
    public class Announcement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnnouncementId { get; set; }

        public int SenderId { get; set; }

        public int ClassId { get; set; }

        public string AnnouncementText { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("SenderId")]
        public User Sender { get; set; }

        [ForeignKey("ClassId")]
        public Class Class { get; set; }
    }
}
