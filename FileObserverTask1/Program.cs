using System;

namespace FileObserverTask1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileSystemVisitor = new FileSystemVisitor();
            fileSystemVisitor.Visit();
            Console.ReadLine();
        }
    }
}
