using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileObserverTask1
{
    public class FileSystemVisitor
    {
        private const string Path = "C:\\Users\\Ivan\\Downloads\\TestFolder";

        public void Visit()
        {
            Console.WriteLine($"Search in {Path}{Environment.NewLine}");

            var result = ProcessPath(Path);

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        string GetName(string path)
        {
            var splitPath = path.Split("\\");
            return splitPath.Last();
        }

        IEnumerable<string> ProcessPath(string directory, string tab = "")
        {

            foreach (var file in Directory.GetFiles(directory))
            {
                yield return ($"{tab}File: {GetName(file)}");
            }

            foreach (var path in Directory.GetDirectories(directory))
            {
                yield return $"{tab}{GetName(path)}";

                foreach (var file in ProcessPath(path, tab + " "))
                {
                    yield return file;
                }
            }
        }
    }
}