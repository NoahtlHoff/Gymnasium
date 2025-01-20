using System;
using Gymnasium.Data;
using Gymnasium.UI.Service;

namespace Gymnasium.UI.Menus
{
    public static class AdminMenu
    {
        public static void RunAdminMenu(GymDbContext context)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Art.Print("Admin Menu", "Select an Option");

                Console.WriteLine("[1] Manage staff");
                Console.WriteLine("[2] Manage students");

                Console.WriteLine("[B] Back [E] Exit");

                string choice = InputHandler.GetMenuChoice("1", "2", "B", "b", "E", "e");

                switch (choice)
                {
                    case "1":
                        ManageStaff(context);
                        break;
                    case "2":
                        ManageStudents(context);
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
        public static void ManageStaff(GymDbContext context)
        {
            bool running = true;
            while (running)
            {
                Art.Print("Manage Staff", "Select an Option");
                Console.WriteLine("[1] Show Staff");
                Console.WriteLine("[2] Add Staff");
                Console.WriteLine("[3] Show Roles");

                Console.WriteLine("[B] Back [E] Exit");
                
                string choice = InputHandler.GetMenuChoice("1", "2", "3", "B", "b", "E", "e");
                switch (choice)
                {
                    case "1":
                        ModelLists.GetStaffList(context);
                        Console.WriteLine("\nPress any key to go back to the menu...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "2":
                        StaffService.AddNewStaff(context);
                        break;
                    case "3":
                        ModelLists.GetRoleList(context);
                        Console.WriteLine("\nPress any key to go back to the menu...");
                        Console.ReadKey();
                        Console.Clear();
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
        public static void ManageStudents(GymDbContext context)
        {
            bool running = true;
            while (running)
            {
                Art.Print("Manage Students", "Select an Option");
                Console.WriteLine("[1] Show Students");
                Console.WriteLine("[2] Add new Student");
                Console.WriteLine("[3] Update Grades");

                Console.WriteLine("[B] Back [E] Exit");

                string choice = InputHandler.GetMenuChoice("1", "2", "3", "B", "b", "E", "e");
                switch (choice)
                {
                    case "1":
                        ModelLists.GetStudentList(context, null);
                        Console.WriteLine("\nPress any key to go back to the menu...");
                        Console.ReadKey();
                        break;
                    case "2":
                        StudentService.AddStudent(context);
                        break;
                    case "3":
                        StudentService.UpdateGrade(context);
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
