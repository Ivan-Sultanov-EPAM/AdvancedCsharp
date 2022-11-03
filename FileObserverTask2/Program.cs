using FileObserverTask1;
using System;

namespace FileObserverTask2
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
                System.Console.WriteLine($"Please enter the path, or press enter to search in Downloads folder:{NewLine}");
                var path = System.Console.ReadLine();

                System.Console.WriteLine($"Please enter the filtering string:{NewLine}");
                var filter = System.Console.ReadLine();

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
                    System.Console.WriteLine($"Something went wrong: {e.Message},{NewLine}please check the provided path");
                }

                System.Console.WriteLine($"{NewLine}Search finished:{NewLine}" +
                                  $"{NewLine}- press enter to repeat search" +
                                  $"{NewLine}- enter \"q\" to quit{NewLine}");

                _exit = System.Console.ReadLine();

            } while (_exit?.ToLower() != "q");
        }
    }
}
