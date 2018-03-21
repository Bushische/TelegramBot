using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TelegramBot.Bot myBOT = new Bot("562165350:AAEbbD2jyYTAKNHTv6KRMgyv37j9PgZbTQs", new List<string>() { "/echo", "/weather" });
                myBOT.OnLog += (x => Console.WriteLine($"Log: {x}"));

                myBOT.StartListening();
                while (true)
                {
                    Console.WriteLine("Press 'x' to exit");
                    var key = Console.ReadKey();
                    if ((key.KeyChar == 'x') || (key.KeyChar == 'X'))
                        break;  // stop work
                }
                myBOT.StopListening();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.ToString()}");
            }
            Console.WriteLine("That's all");
            Console.ReadKey();
        }
    }
}
