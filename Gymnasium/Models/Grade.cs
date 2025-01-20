using System;
using System.Collections.Generic;

namespace Gymnasium.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int? SubjectId { get; set; } 
    public Subject Subject { get; set; } 

    public int? StudentId { get; set; }
    public Student Student { get; set; }

    public string? GradeValue { get; set; }
    public DateTime? Date { get; set; }

    public virtual GradeScale? GradeValueNavigation { get; set; }
}

