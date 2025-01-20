using System;
using System.Collections.Generic;

namespace Gymnasium.Models;

public class Subject
{
    public int SubjectId { get; set; } // Previously CourseID
    public string SubjectName { get; set; } // Previously CourseName

    // Foreign key for the teacher
    public int? TeacherId { get; set; }
    public Staff? Teacher { get; set; }

    // Navigation property for Grades
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}

