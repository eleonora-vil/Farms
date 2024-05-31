using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos
{
    public class PermissionModel
    {
        public int PermissionId { get; set; }

        [StringLength(20)]
        public string SyllabusAccess { get; set; }

        [StringLength(20)]
        public string ProgramAccess { get; set; }

        [StringLength(20)]
        public string UserAccess { get; set; }

        [StringLength(20)]
        public string ClassAccess { get; set; }

        [StringLength(20)]
        public string MaterialAccess { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public virtual UserRole Role { get; set; }
    }
}
