using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Models
{
    public class RoleMenu
    {
        [Required]
        public string RoleId { get; set; } = string.Empty;
        
        [Required]
        public string MenuId { get; set; } = string.Empty;
        
        public Menu Menu { get; set; } = null!;
    }
}