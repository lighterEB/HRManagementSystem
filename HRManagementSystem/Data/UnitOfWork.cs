using System;
using System.Threading.Tasks;
using HRManagementSystem.Data.Interfaces;
using HRManagementSystem.Data.Repositories;
using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace HRManagementSystem.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IRepository<Announcement>? _announcements;
    private IRepository<Attendance>? _attendances;
    private IRepository<Department>? _departments;
    private bool _disposed;

    private IRepository<Employee>? _employees;
    private IRepository<LeaveRequest>? _leaveRequests;
    private IRepository<Position>? _positions;
    private IRepository<Salary>? _salaries;
    private IRepository<TodoItem>? _todoItems;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<Employee> Employees =>
        _employees ??= new EfRepository<Employee>(_context);

    public IRepository<Department> Departments =>
        _departments ??= new EfRepository<Department>(_context);

    public IRepository<Position> Positions =>
        _positions ??= new EfRepository<Position>(_context);

    public IRepository<Attendance> Attendances =>
        _attendances ??= new EfRepository<Attendance>(_context);

    public IRepository<LeaveRequest> LeaveRequests =>
        _leaveRequests ??= new EfRepository<LeaveRequest>(_context);

    public IRepository<Salary> Salaries =>
        _salaries ??= new EfRepository<Salary>(_context);

    public IRepository<Announcement> Announcements =>
        _announcements ??= new EfRepository<Announcement>(_context);

    public IRepository<TodoItem> TodoItems =>
        _todoItems ??= new EfRepository<TodoItem>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null)
                await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
        await DisposeAsync(true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async Task DisposeAsync(bool disposing)
    {
        if (!_disposed && disposing)
        {
            if (_transaction != null)
                await _transaction.DisposeAsync();
            await _context.DisposeAsync();
        }

        _disposed = true;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        _disposed = true;
    }
}