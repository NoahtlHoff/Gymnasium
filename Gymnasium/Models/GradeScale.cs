using System;
using System.Collections.Generic;

namespace Gymnasium.Models;

public partial class GradeScale
{
    public string GradeValue { get; set; } = null!;

    public int GradeRank { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
