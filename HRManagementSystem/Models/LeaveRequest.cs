using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagementSystem.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        
        [Required]
        [StringLength(20)]
        public string LeaveType { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(5,1)")]
        public decimal Duration { get; set; }
        
        [StringLength(500)]
        public string? Reason { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        
        public int? ApprovedById { get; set; }
        public Employee? ApprovedBy { get; set; }
        
        public DateTime? ApprovalDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public string? CreatedById { get; set; }
        public User? CreatedBy { get; set; }
    }
}