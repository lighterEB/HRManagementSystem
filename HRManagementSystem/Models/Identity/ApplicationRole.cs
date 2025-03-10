using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.Models.Identity;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
        RoleMenus = new HashSet<RoleMenu>();
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
        RoleMenus = new HashSet<RoleMenu>();
    }

    // 角色描述
    public string Description { get; set; } = string.Empty;

    // 是否为系统角色（不可删除）
    public bool IsSystemRole { get; set; }

    // 修改为可空整型
    public int? MenuId { get; set; } = null;

    // 确保RoleId不会造成冲突
    public string? RoleId { get; set; } = null;
    
    // 导航属性
    public virtual ICollection<RoleMenu> RoleMenus { get; set; }
}