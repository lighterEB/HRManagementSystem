using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Models
{
    public class RolePermission
    {
        [Required]
        public string RoleId { get; set; } = string.Empty;
        
        [Required]
        public string PermissionId { get; set; } = string.Empty;
        
        public Permission Permission { get; set; } = null!;
    }
}