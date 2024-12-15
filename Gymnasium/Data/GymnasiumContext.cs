using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gymnasium.Models;
using Microsoft.EntityFrameworkCore;

public class GymnasiumContext : DbContext
{
    public DbSet<Personal> Personals { get; set; }
    public DbSet<Students> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<GradeScale> GradeScales { get; set; }
    public DbSet<Class> Classes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-JAEBKSV;Initial Catalog=Gymnasium;Integrated Security=True;");
    }
}