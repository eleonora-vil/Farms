using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Entities
{
    [Table("Permission")]
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

        [Required]
        [StringLength(20)]
        public string SyllabusAccess { get; set; }

        [Required]
        [StringLength(20)]
        public string ProgramAccess { get; set; }

        [Required]
        [StringLength(20)]
        public string UserAccess { get; set; }

        [Required]
        [StringLength(20)]
        public string ClassAccess { get; set; }

        [Required]
        [StringLength(20)]
        public string MaterialAccess { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public virtual UserRole Role { get; set; }

    }
}
