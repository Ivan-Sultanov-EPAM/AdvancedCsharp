using System;

namespace FileObserverTask1
{
    internal class Program
    {
        private static readonly string Path = GetDownloadFolder.GetFolder();
        private static string _exit;
        private static readonly string NewLine = Environment.NewLine;

        private static void Main()
        {
            do
            {
                Console.WriteLine($"Please enter the path, or press enter to search in Downloads folder:{NewLine}");
                var path = Console.ReadLine();

                Console.WriteLine($"Please enter the filtering string:{NewLine}");
                var filter = Console.ReadLine();

                try
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        var visitor = new FileSystemVisitor(string.IsNullOrEmpty(path) ? Path : path,
                            x => x.ToLower().Contains(filter.ToLower()));

                        visitor.Search();
                    }
                    else
                    {
                        var visitor = new FileSystemVisitor(string.IsNullOrEmpty(path) ? Path : path);

                        visitor.Search();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong: {e.Message},{NewLine}please check the provided path");
                }

                Console.WriteLine($"{NewLine}- press enter to repeat search" +
                                  $"{NewLine}- enter \"q\" to quit{NewLine}");

                _exit = Console.ReadLine();

            } while (_exit?.ToLower() != "q");
        }
    }
}
