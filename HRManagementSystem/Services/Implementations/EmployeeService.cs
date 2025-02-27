using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRManagementSystem.Data;
using HRManagementSystem.Models;
using HRManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetEmployeeAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.Manager)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return Enumerable.Empty<Employee>();
        var normalizedKeyword = keyword.Trim();
        return await _context.Employees
            .Where(e =>
                EF.Functions.Like(e.FirstName, $"%{normalizedKeyword}%") ||
                EF.Functions.Like(e.LastName, $"%{normalizedKeyword}%") ||
                EF.Functions.Like(e.Email, $"%{normalizedKeyword}%") ||
                (e.PhoneNumber != null && EF.Functions.Like(e.PhoneNumber, $"%{normalizedKeyword}%")))
            .ToListAsync();
    }

    public async Task CreateEmployeeAsync(Employee employee)
    {
        // 生成员工编号（示例逻辑）
        employee.EmployeeNumber = GenerateEmployeeNumber();

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _context.Entry(employee).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AssignDepartmentAsync(int employeeId, int departmentId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        var department = await _context.Departments.FindAsync(departmentId);

        if (employee != null && department != null)
        {
            employee.Department = department;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AssignPositionAsync(int employeeId, int positionId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        var position = await _context.Positions.FindAsync(positionId);

        if (employee != null && position != null)
        {
            employee.Position = position;
            await _context.SaveChangesAsync();
        }
    }

    private string GenerateEmployeeNumber()
    {
        // 示例生成规则：年份+月份+4位序列号
        var now = DateTime.Now;
        var maxNumber = _context.Employees
            .Where(e => e.HireDate.Year == now.Year && e.HireDate.Month == now.Month)
            .Max(e => (int?)e.Id) ?? 0;

        return $"{now:yyyyMM}-{maxNumber + 1:0000}";
    }
}