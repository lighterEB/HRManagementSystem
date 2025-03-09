using System.ComponentModel.DataAnnotations;
using HRManagementSystem.Models.Identity;

namespace HRManagementSystem.Models;

public class RoleMenu
{
    [Required] public string RoleId { get; set; } = string.Empty;

    [Required] public string MenuId { get; set; } = string.Empty;

    public virtual ApplicationRole Role { get; set; } = null!;
    public virtual Menu Menu { get; set; } = null!;
}