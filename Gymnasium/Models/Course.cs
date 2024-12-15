using System;
using System.Collections.Generic;

namespace Gymnasium.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public int? TeacherId { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Staff? Teacher { get; set; }
}
