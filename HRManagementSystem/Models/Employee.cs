using System;
using System.ComponentModel.DataAnnotations;
using HRManagementSystem.Models.Identity;

namespace HRManagementSystem.Models;

public class Employee
{
    public int Id { get; set; }

    [Required] [StringLength(20)] public string EmployeeNumber { get; set; } = string.Empty;

    [Required] [StringLength(50)] public string FirstName { get; set; } = string.Empty;

    [Required] [StringLength(50)] public string LastName { get; set; } = string.Empty;

    [StringLength(10)] public string? Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    [StringLength(18)] public string? IdCardNumber { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)] public string? PhoneNumber { get; set; }

    [StringLength(255)] public string? Address { get; set; }

    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? PositionId { get; set; }
    public Position? Position { get; set; }

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    [Required] public DateTime HireDate { get; set; }

    [Required] [StringLength(20)] public string Status { get; set; } = "Active";

    public DateTime? TerminationDate { get; set; }

    [StringLength(255)] public string? Photo { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}