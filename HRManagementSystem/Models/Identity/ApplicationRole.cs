using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.Models.Identity;

public class ApplicationRole : IdentityRole
{
    // 角色描述
    public string Description { get; set; } = string.Empty;

    // 是否系统内置角色
    public bool IsSystemRole { get; set; }

    // 权限集合
    public virtual ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
}