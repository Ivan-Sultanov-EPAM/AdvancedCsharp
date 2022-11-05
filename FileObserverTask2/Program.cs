using FileObserverTask1;
using System;
using Console = System.Console;

namespace FileObserverTask2
{
    internal class Program
    {
        private static readonly string Path = GetDownloadFolder.GetFolder();
        private static readonly string NewLine = Environment.NewLine;
        private static string _exit;

        private static void Main()
        {
            do
            {
                Console.WriteLine("Please enter the path, or press enter to search in Downloads folder:");
                var path = Console.ReadLine();

                Console.WriteLine("Please enter the filtering string:");
                var filter = Console.ReadLine();

                try
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        var visitor = new FileSystemVisitor(string.IsNullOrEmpty(path) ? Path : path,
                            x => x.ToLower().Contains(filter.ToLower()));

                        visitor.EventHandler += MessageHandler;
                        visitor.ActionHandler += ActionHandler;

                        visitor.Search();
                    }
                    else
                    {
                        var visitor = new FileSystemVisitor(string.IsNullOrEmpty(path) ? Path : path);

                        visitor.EventHandler += MessageHandler;
                        visitor.ActionHandler += ActionHandler;

                        visitor.Search();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong: {e.Message},{NewLine}please check the provided path");
                }

                Console.WriteLine($"{NewLine}Search finished:{NewLine}" +
                                  $"{NewLine}- press enter to repeat search" +
                                  $"{NewLine}- enter \"q\" to quit");

                _exit = Console.ReadLine();

            } while (_exit?.ToLower() != "q");
        }

        private static void MessageHandler(string x)
        {
            Console.WriteLine($"{x}");
        }

        private static ActionsEnum ActionHandler()
        {
            Console.WriteLine(
                $"Proceed: press enter{NewLine}" +
                $"Include: enter 1{NewLine}" +
                $"Exclude: enter 2{NewLine}" +
                "Stop search: enter 3"
            );

            var result = Console.ReadLine();

            return result switch
            {
                "1" => ActionsEnum.Include,
                "2" => ActionsEnum.Exclude,
                "3" => ActionsEnum.StopSearch,
                _ => ActionsEnum.Proceed,
            };
        }
    }
}
