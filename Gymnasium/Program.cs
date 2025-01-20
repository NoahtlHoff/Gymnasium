using System;
using System.IO;
using Gymnasium.Data;
using Gymnasium.Models;
using Gymnasium.UI;
using Gymnasium.UI.Service;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = File.ReadAllText(@"C:\connectionString.txt").Trim();

        var optionsBuilder = new DbContextOptionsBuilder<GymDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        using (var context = new GymDbContext(optionsBuilder.Options))
        {
            // Check Students, if there is no data in table, add large sampleData
            bool hasStudents = context.Students.Any();
            if (!hasStudents)
            {
                SampleData.Add(context);
            }
            //Starts the program:
            Run.StartUpMenu(context);

        }
    }
}