using System;
using System.ComponentModel.DataAnnotations;
using HRManagementSystem.Models.Identity;

namespace HRManagementSystem.Models;

public class Attendance
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    [Required] public DateTime Date { get; set; }

    public TimeSpan? CheckInTime { get; set; }

    public TimeSpan? CheckOutTime { get; set; }

    [StringLength(20)] public string? Status { get; set; }

    [StringLength(255)] public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? CreatedById { get; set; }
    public User? CreatedBy { get; set; }
}