using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gymnasium.Data;
using Gymnasium.UI.Menus;
using Gymnasium.UI.Service;
using Microsoft.Extensions.Options;

namespace Gymnasium.UI
{
    public class Run
    {
        public static void StartUpMenu(GymDbContext context)
        {
            bool running = true;
            while (running)
            {
                Art.Print("Main Menu", "Select an option");
                Console.WriteLine("[1] Log in as Admin");
                Console.WriteLine("[2] Log in as Teacher");
                Console.WriteLine("[3] Log in as Student");
                Console.WriteLine("[E] Exit");

                string choice = InputHandler.GetMenuChoice("1", "2", "3", "E", "e");

                switch (choice)
                {
                    case "1":
                        AdminMenu.RunAdminMenu(context);
                        break;
                    case "2":
                        AdminMenu.ManageStudents(context);
                        break;
                    case "3":
                        StudentMenu.Identity(context);
                        break;
                    case "e":
                    case "E":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice, try again.");
                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
