using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRManagementSystem.Data;
using HRManagementSystem.Models;
using HRManagementSystem.Models.Identity;
using HRManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services.Implementations;

public class MenuService : IMenuService
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public MenuService(UserManager<User> userManager, AppDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Menu>> GetUserMenusAsync(string userId)
    {
        // 1. 获取用户角色
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return Enumerable.Empty<Menu>();

        var roles = await _userManager.GetRolesAsync(user);

        // 2. 获取角色关联的菜单
        var menus = await _dbContext.RoleMenus
            .Include(rm => rm.Menu)
            .Where(rm => roles.Contains(rm.Role.Name))
            .Select(rm => rm.Menu)
            .Distinct()
            .OrderBy(m => m.OrderIndex)
            .ToListAsync();

        // 3. 构建菜单树
        return BuildMenuTree(menus);
    }

    public async Task<bool> HasPermissionAsync(string userId, string permissionCode)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var roles = await _userManager.GetRolesAsync(user);

        return await _dbContext.RolePermissions
            .Include(rp => rp.Permission)
            .AnyAsync(rp => roles.Contains(rp.RoleId) &&
                            rp.Permission.Code == permissionCode);
    }

    public async Task<IEnumerable<Menu>> GetAllMenusAsync()
    {
        var menus = await _dbContext.Menus
            .OrderBy(m => m.OrderIndex)
            .ToListAsync();

        return BuildMenuTree(menus);
    }

    public async Task<Menu?> GetMenuByIdAsync(string menuId)
    {
        return await _dbContext.Menus
            .Include(m => m.ChildMenus)
            .FirstOrDefaultAsync(m => m.Id == menuId);
    }

    public async Task<Menu> CreateMenuAsync(Menu menu)
    {
        menu.Id = Guid.NewGuid().ToString();
        menu.CreatedAt = DateTime.UtcNow;
        menu.UpdatedAt = DateTime.UtcNow;

        _dbContext.Menus.Add(menu);
        await _dbContext.SaveChangesAsync();

        return menu;
    }

    public async Task<Menu?> UpdateMenuAsync(Menu menu)
    {
        var existingMenu = await _dbContext.Menus.FindAsync(menu.Id);
        if (existingMenu == null) return null;

        existingMenu.Name = menu.Name;
        existingMenu.Icon = menu.Icon;
        existingMenu.Url = menu.Url;
        existingMenu.Component = menu.Component;
        existingMenu.ParentId = menu.ParentId;
        existingMenu.OrderIndex = menu.OrderIndex;
        existingMenu.PermissionCode = menu.PermissionCode;
        existingMenu.IsVisible = menu.IsVisible;
        existingMenu.IsActive = menu.IsActive;
        existingMenu.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return existingMenu;
    }

    public async Task<bool> DeleteMenuAsync(string menuId)
    {
        var menu = await _dbContext.Menus
            .Include(m => m.ChildMenus)
            .FirstOrDefaultAsync(m => m.Id == menuId);

        if (menu == null) return false;

        // 递归删除子菜单
        foreach (var childMenu in menu.ChildMenus.ToList()) await DeleteMenuAsync(childMenu.Id);

        _dbContext.Menus.Remove(menu);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private IEnumerable<Menu> BuildMenuTree(IEnumerable<Menu> menus)
    {
        var lookup = menus.ToLookup(m => m.ParentId);
        return BuildMenuTreeRecursive(null, lookup);
    }

    private IEnumerable<Menu> BuildMenuTreeRecursive(string? parentId, ILookup<string?, Menu> lookup)
    {
        foreach (var menu in lookup[parentId])
        {
            menu.ChildMenus = BuildMenuTreeRecursive(menu.Id, lookup).ToList();
            yield return menu;
        }
    }
}