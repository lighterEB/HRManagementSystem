using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Models;

public class Department
{
    public int Id { get; set; }

    [Required] [StringLength(100)] public string Name { get; set; } = string.Empty;

    [StringLength(20)] public string? Code { get; set; }

    [StringLength(500)] public string? Description { get; set; }

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public int? ParentDepartmentId { get; set; }
    public Department? ParentDepartment { get; set; }

    public ICollection<Department> ChildDepartments { get; set; } = new List<Department>();

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}