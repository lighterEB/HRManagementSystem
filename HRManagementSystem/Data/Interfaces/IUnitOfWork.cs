using System;
using System.Threading.Tasks;
using HRManagementSystem.Models;

namespace HRManagementSystem.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Employee> Employees { get; }
    IRepository<Department> Departments { get; }
    IRepository<Position> Positions { get; }
    IRepository<Attendance> Attendances { get; }
    IRepository<LeaveRequest> LeaveRequests { get; }
    IRepository<Salary> Salaries { get; }
    IRepository<Announcement> Announcements { get; }
    IRepository<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}