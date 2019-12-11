using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGenerator;

namespace ConsoleTestGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Write list of test separating them by spaces");
                var testFiles = Console.ReadLine().Split(' ').ToList();
                Console.WriteLine("Write test folder");
                var folder = Console.ReadLine();
                Console.WriteLine("Write max amount of streams for reading");
                var maxInputStreams = int.Parse(Console.ReadLine());
                Console.WriteLine("Write max amount of streams for writing");
                var maxOutStreams = int.Parse(Console.ReadLine());
                Console.WriteLine("Write max amount of streams for generating tests");
                var maxMainStreams = int.Parse(Console.ReadLine());
                new Generator(testFiles, folder, maxInputStreams, maxOutStreams, maxMainStreams).GenerateAsync().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
