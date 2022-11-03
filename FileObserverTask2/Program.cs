using System;

namespace FileObserverTask2
{
    public class Program
    {
        private const string Path = "C:\\Users\\Ivan\\Downloads\\TestFolder";
        private static string _exit;
        private static readonly string _newLine = Environment.NewLine;

        private static void Main(string[] args)
        {
            do
            {
                Console.WriteLine($"Please enter the path:{_newLine}");
                var path = Console.ReadLine();

                Console.WriteLine($"Please enter the filtering string:{_newLine}");
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
                    Console.WriteLine($"Something went wrong: {e.Message},{_newLine}please check the provided path");
                }

                Console.WriteLine($"{_newLine}Search finished:{_newLine}" +
                                  $"{_newLine}- press enter to repeat" +
                                  $"{_newLine}- enter \"exit\" to quit{_newLine}");

                _exit = Console.ReadLine();

            } while (_exit?.ToLower() != "exit");
        }
    }
}
