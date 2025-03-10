using System.Collections.Generic;
using System.Threading.Tasks;
using HRManagementSystem.Models;

namespace HRManagementSystem.Services.Interfaces;

public interface IMenuService
{
    Task<IEnumerable<Menu>> GetUserMenusAsync(string userId);
    Task<bool> HasPermissionAsync(string userId, string permissionCode);
    Task<IEnumerable<Menu>> GetAllMenusAsync();
    Task<Menu?> GetMenuByIdAsync(string menuId);
    Task<Menu> CreateMenuAsync(Menu menu);
    Task<Menu?> UpdateMenuAsync(Menu menu);
    Task<bool> DeleteMenuAsync(string menuId);
}