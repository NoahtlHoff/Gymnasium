using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gymnasium.UI
{
    public class Art
    {
        public static void Print(string navText, string userCommand)
        {
            int boarderLength = 48;
            int textLength = navText.Length;

            if (boarderLength >= textLength)
            {
                Console.Write("┌");
                Console.Write(new string('─', (boarderLength - textLength) / 2));
                Console.Write(navText);
                Console.Write(new string('─', (boarderLength - textLength) / 2));
                Console.WriteLine("┐ \n");
            }
            else
            {
                Console.Write("├");
                Console.Write(new string('─', boarderLength));
                Console.WriteLine("┤\n");
            }

            Console.WriteLine("        __...--~~~~~-._THE_.-~~~~~--...__\r\n       //              `V'              \\\\ \r\n      //   THE GENIUS   |   GYMNASIUM    \\\\    \r\n     //                 |                 \\\\ \r\n    //__...--~~~~~~-._  |  _.-~~~~~~--...__\\\\ \r\n   //__.....----~~~~._\\ | /_.~~~~----.....__\\\\\r\n  ====================\\\\|//====================\r\n                      `---`");

            Console.Write( new string(' ', (boarderLength/2)-4));
            Console.WriteLine("- INFO -");

            textLength = userCommand.Length;
            if (boarderLength >= textLength)
            {
                Console.Write("├");
                Console.Write(new string('─', (boarderLength - textLength) / 2));
                Console.Write(userCommand);
                Console.Write(new string('─', (boarderLength - textLength) / 2));
                Console.WriteLine("┤\n");
                return;
            }
            else
            {
                Console.Write("├");
                Console.Write(new string('─', boarderLength));
                Console.WriteLine("┤\n");
                Console.WriteLine();
                return;
            }
        }
    }
}
