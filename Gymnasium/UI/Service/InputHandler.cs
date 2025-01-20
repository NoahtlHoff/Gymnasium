using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;

namespace Gymnasium.UI.Service
{
    internal class InputHandler
    {
        private static (int Left, int Top)? promptPos = null;
        private static (int Left, int Top)? infoboxPos = null;
        private static (int Left, int Top)? Bottom = null;
        public static string InputPrompt()
        {
            Console.WriteLine("\n\n");
            Bottom = Console.GetCursorPosition();
            Console.SetCursorPosition(Bottom.Value.Left, Bottom.Value.Top - 1);
            Console.Write("User input: ");

            promptPos = Console.GetCursorPosition(); // Capture prompt position 


            infoboxPos = (0, promptPos.Value.Top - 1); // Calculate infobox position

            Console.SetCursorPosition(promptPos.Value.Left, promptPos.Value.Top);

            string input = Console.ReadLine();

            return input;
        }

        public static void Infobox()
        {
            string ErrorMessage = "- INVALID CHOICE -";

            if (promptPos == null || infoboxPos == null)
            {
                throw new InvalidOperationException("InputPrompt must be called before Infobox.");
            }

            // Clear the prompt line
            Console.SetCursorPosition(promptPos.Value.Left, promptPos.Value.Top);
            Console.Write(new string(' ', Console.WindowWidth - promptPos.Value.Left));

            // Display info to user in the infobox
            Console.SetCursorPosition(infoboxPos.Value.Left, infoboxPos.Value.Top);
            Console.Write(ErrorMessage);

            // Wait to show the message
            Thread.Sleep(1500);

            // Clear the infobox
            Console.SetCursorPosition(infoboxPos.Value.Left, infoboxPos.Value.Top);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(promptPos.Value.Left, promptPos.Value.Top-2);
        }
        public static T ReturnCleaner<T>(T value)
        {
            Thread.Sleep(1000);
            Console.Clear();
            return value;   
        }
        public static string GetMenuChoice(params string[] validChoices)
        {
            while (true)
            {
                string input = InputPrompt();
                if (validChoices.Contains(input))
                {
                    return ReturnCleaner(input);
                }
                Infobox();
            }
        }
        public static string GetValidGrade(params string[] validChoices)
        {
            while (true)
            {
                string input = InputPrompt();
                if (validChoices.Contains(input))
                {
                    return ReturnCleaner(input);
                }
                Infobox();
            }
        }
        public static int GetValidInteger(int min, int max)
        {
            while (true)
            {
                string input = InputPrompt();
                if (int.TryParse(input, out int number))
                {
                    if (number >= min && number <= max)
                    {
                        return ReturnCleaner(number);
                    }
                }
                Infobox();
            }
        }
        public static string GetValidString(int maxLength)
        {
            while (true)
            {
                string input = InputPrompt().Trim();

                if (!string.IsNullOrWhiteSpace(input) && input.Length <= maxLength)
                {
                    return ReturnCleaner(input);
                }
                Infobox();
            }
        }

        public static decimal GetValidDecimal(decimal min = 0, decimal max = decimal.MaxValue)
        {
            while (true)
            {
                string input = InputPrompt();
                
                if (decimal.TryParse(input, out var number) && number >= min && number <= max)
                {
                    return ReturnCleaner(number);
                }
                Infobox();
            }
        }

        public static DateTime GetValidDate()
        {
            while (true)
            {
                string input = InputPrompt();

                if (DateTime.TryParse(input, out var date))
                {
                    return ReturnCleaner(date);
                }
                Infobox();
            }
        }
        //Console.Write($"Enter the ID {prompt}: ");
        //int promptCol = Console.CursorLeft;
        //Console.WriteLine();
        //int infobox = Console.CursorTop - 2;
        //int promptLine = Console.CursorTop -1;
        public static int GetValidID<T>(IEnumerable<T> model, Func<T, int> idSelector)
        {
            while (true)
            {
                string input = InputPrompt();
                //promptPos = Console.GetCursorPosition();
                //Console.SetCursorPosition(promptPos.Value.Left, promptPos.Value.Top-2);
                if (int.TryParse(input, out int ID))
                {
                    var validID = model.FirstOrDefault(m => idSelector(m) == ID);

                    if (validID != null)
                    {
                        return ReturnCleaner(ID);
                    }
                }
                Infobox();
            }
        }
    }
}
    