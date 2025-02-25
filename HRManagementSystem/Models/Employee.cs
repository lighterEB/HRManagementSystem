using System;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; init; }

        [Required]
        [StringLength(20)]
        public string EmployeeNumber { get; init; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; init; }

        public DateTime HireDate { get; init; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public int PositionId { get; set; }
        public Position Position { get; set; } = null!;

        public string? UserId { get; set; }
        public User? User { get; set; }

        public EmployeeStatus Status { get; set; }

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }

    public enum EmployeeStatus
    {
        Active,
        OnLeave,
        Terminated
    }
}