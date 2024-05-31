using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Dtos
{
    public class UserRoleModel
    {
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
    }
}
