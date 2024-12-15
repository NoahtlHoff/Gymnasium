using System;
using System.Collections.Generic;

namespace Gymnasium.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int? CourseId { get; set; }

    public int? StudentId { get; set; }

    public string? GradeValue { get; set; }

    public DateTime? Date { get; set; }

    public virtual Course? Course { get; set; }

    public virtual GradeScale? GradeValueNavigation { get; set; }

    public virtual Student? Student { get; set; }
}
