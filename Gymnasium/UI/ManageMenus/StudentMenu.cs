using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Gymnasium.Data;
using Gymnasium.UI.Service;
using Microsoft.Identity.Client;

namespace Gymnasium.UI.Menus
{
    internal class StudentMenu
    {
        public static void Identity(GymDbContext context)
        {
            bool running = true;
            while (running)
            {
                Art.Print("Identity Check", "Enter studentID");

                Console.WriteLine("[L] Print student-list");
                Console.WriteLine("[B] Back [E] Exit");

                string input = InputHandler.InputPrompt();

                var students = context.Students
                .Select(s => new
                {
                    s.StudentId,
                })
                .ToList();

                if (int.TryParse(input, out int ID))
                {
                    var validID = students.FirstOrDefault(s => s.StudentId == ID);

                    if (validID != null)
                    {
                        Thread.Sleep(1000);
                        Console.Clear();
                        StudentOverlook(context, ID);
                    }
                    else { InputHandler.Infobox(); Console.Clear(); }
                }
                else if (input == "L" || input == "l" || input == "B" || input == "b" || input == "E" || input == "e")
                {
                    switch (input)
                    {
                        case "L":
                        case "l":
                            Console.Clear();
                            ModelLists.GetStudentList(context);
                            break;
                        case "B":
                        case "b":
                            Console.Clear();
                            running = false;
                            break;
                        case "E":
                        case "e":
                            Console.Clear();
                            Console.WriteLine("Program is closing...");
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    InputHandler.Infobox();
                    Console.Clear();
                }

            }
        }
        public static void StudentOverlook(GymDbContext context, int ID)
        {
            // For example, to get all information you need about the student in question:
            var student = context.Students
                .Where(s => s.StudentId == ID)
                .Select(s => new
                {
                    s.StudentId,
                    s.FirstName,
                    s.LastName,
                    s.ClassId
                })
                .FirstOrDefault();

            if (student == null)
            {
                Console.WriteLine("Student not found. Returning to previous menu...");
                return;
            }

            bool running = true;
            while (running)
            {
                Art.Print($"{student.FirstName}'s Menu", "Select an Option");

                Console.WriteLine("[1] Show classmates");
                Console.WriteLine("[2] Show Subjects and grades");
                Console.WriteLine("[B] Back [E] Exit");

                string choice = InputHandler.GetMenuChoice("1", "2", "B", "b", "E", "e");
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"); //temporary solution because console clear doesnt reset the console apparently
                        ModelLists.GetStudentList(context, student.ClassId);
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                        ModelLists.GetSubjectList(context, student.StudentId);
                        break;
                    case "B":
                    case "b":
                        running = false;
                        break;

                    case "E":
                    case "e":
                        Console.WriteLine("Program is closing...");
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}