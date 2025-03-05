using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Models;

public class Menu
{
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required] [StringLength(100)] public string Name { get; set; } = string.Empty;

    [StringLength(50)] public string? Icon { get; set; }

    [StringLength(255)] public string? Url { get; set; }

    [StringLength(255)] public string? Component { get; set; }

    public string? ParentId { get; set; }

    public Menu? ParentMenu { get; set; }

    public ICollection<Menu> ChildMenus { get; set; } = new List<Menu>();

    public int OrderIndex { get; set; }

    [StringLength(50)] public string? PermissionCode { get; set; }

    public bool IsVisible { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<RoleMenu> RoleMenus { get; set; } = new HashSet<RoleMenu>();
}