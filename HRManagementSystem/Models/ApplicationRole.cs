using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        
        public ApplicationRole(string roleName) : base(roleName) { }
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        public bool IsSystemRole { get; set; }
    }
}