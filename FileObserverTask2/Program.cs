using System;

namespace FileObserverTask2
{
    internal class Program
    {
        private const string Path = "C:\\Users\\Ivan\\Downloads\\TestFolder";
        private static string _exit;

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine($"Please enter the path:{Environment.NewLine}");
                var path = Console.ReadLine();

                Console.WriteLine($"Please enter the filtering string:{Environment.NewLine}");
                var filter = Console.ReadLine();

                try
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        var visitor = new FileSystemVisitor(string.IsNullOrEmpty(path) ? Path : path,
                            x => x.Contains(filter.ToLower()));

                        visitor.Visit();
                    }
                    else
                    {
                        var visitor = new FileSystemVisitor(string.IsNullOrEmpty(path) ? Path : path);

                        visitor.Visit();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong: {e.Message},{Environment.NewLine}please check the provided path");
                }

                Console.WriteLine($"{Environment.NewLine}Press enter to repeat{Environment.NewLine}" +
                                  $"or type \"exit\" and press enter to quit{Environment.NewLine}");

                _exit = Console.ReadLine();

            } while (_exit?.ToLower() != "exit");
        }
    }
}
