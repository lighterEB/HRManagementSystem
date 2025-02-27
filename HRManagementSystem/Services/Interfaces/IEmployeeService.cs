using System.Collections.Generic;
using System.Threading.Tasks;
using HRManagementSystem.Models;

namespace HRManagementSystem.Services.Interfaces;

public interface IEmployeeService
{
    Task<Employee?> GetEmployeeAsync(int id);
    Task<IEnumerable<Employee>> SearchEmployeesAsync(string keyword);
    Task CreateEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int id);
    Task AssignDepartmentAsync(int employeeId, int departmentId);
    Task AssignPositionAsync(int employeeId, int positionId);
}