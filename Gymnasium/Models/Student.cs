using System;
using System.Collections.Generic;

namespace Gymnasium.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? ClassId { get; set; }

    public virtual Class? Class { get; set; }
    public DateTime DateOfBirth { get; set; }
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
