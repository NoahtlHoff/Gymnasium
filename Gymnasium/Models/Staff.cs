using System;
using System.Collections.Generic;

namespace Gymnasium.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Position { get; set; }

    public string? PersonalNumber { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
