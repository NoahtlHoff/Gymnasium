using System;
using System.Linq;
using Gymnasium.Data;
using Gymnasium.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Gymnasium.UI.Service
{
    internal class StaffService
    {
        // Prints staff
       

        // Create new staff
        public static void AddNewStaff(GymDbContext context)
        {
            Art.Print("Adding Staff", "Enter First Name");
            var firstName = InputHandler.GetValidString(35);

            Art.Print("Adding Staff", "Enter Last Name");
            var lastName = InputHandler.GetValidString(35);

            Art.Print("Adding Staff", "Enter Role");
            var role = InputHandler.GetValidString(50);

            Art.Print("Adding Staff", "Enter Salary");
            var salary = InputHandler.GetValidDecimal( 0, 120000);

            Art.Print("Adding Staff", "Enter Start Date [yyyy-MM-dd]");
            var startDate = InputHandler.GetValidDate();

            Art.Print("Adding Staff", "Enter Birth Date [yyyy-MM-dd]");
            var dateOfBirth = InputHandler.GetValidDate();

            Art.Print("SQL QUERY PRESENTATION", "Press Y or N");
            // Display SQL query
            string sqlQuery = $@"INSERT INTO Staff (FirstName, LastName, Role, Salary, StartDate, DateOfBirth)
VALUES ('{firstName.Replace("'", "''")}', '{lastName.Replace("'", "''")}', '{role.Replace("'", "''")}', {salary}, '{startDate:yyyy-MM-dd}', '{dateOfBirth:yyyy-MM-dd}');";


            Console.WriteLine("\n(How the SQL query in SSMS would look):");
            Console.WriteLine(sqlQuery);
            Console.WriteLine("\nDo you want to execute this query? (Y/N)");

            var execute = InputHandler.GetMenuChoice("Y", "N", "y", "n");
            if (execute == "N" || execute == "n")
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            var newStaff = new Staff
            {
                FirstName = firstName,
                LastName = lastName,
                Role = role,
                Salary = salary,
                StartDate = startDate,
                DateOfBirth = dateOfBirth
            };

            try
            {
                context.Staff.Add(newStaff);
                context.SaveChanges();
                Console.WriteLine("New staff member added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the staff member: {ex.Message}");
            }
            Thread.Sleep(1000);
        }
    }
}
