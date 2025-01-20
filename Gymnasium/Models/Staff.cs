using System;
using System.Collections.Generic;

namespace Gymnasium.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Role { get; set; }  // Use this field for "Teacher", "Admin", etc.

    public decimal Salary { get; set; } 
    public DateTime StartDate { get; set; }

    public DateTime DateOfBirth { get; set; }  
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
