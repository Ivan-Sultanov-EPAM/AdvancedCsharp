using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileObserverTask1
{
    public class FileSystemVisitor
    {
        private readonly string _path;
        public delegate bool Filter(string value);
        private static Filter _filter;
        private static readonly string NewLine = Environment.NewLine;

        public FileSystemVisitor(string path)
        {
            _path = path;
            _filter = null;
        }

        public FileSystemVisitor(string path, Filter filter)
        {
            _path = path;
            _filter = filter;
        }

        public void Search()
        {
            Console.WriteLine($"Search in {_path}{NewLine}" +
                              $"{NewLine}--------- Search Result -----------{NewLine}");
            var count = 0;

            foreach (var item in ProcessPath(_path))
            {
                Console.WriteLine(item);
                count++;
            }

            if (count == 0)
                Console.WriteLine("\t    No results");

            Console.WriteLine($"{NewLine}-------- Search Finished ------------{NewLine}");
        }

        private static IEnumerable<string> ProcessPath(string directory, string tab = "")
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                var fileName = GetName(file);

                if (GetFilteredResult(fileName))
                {
                    yield return $"{tab}{fileName}";
                }
            }

            foreach (var path in Directory.GetDirectories(directory))
            {
                if (GetFilteredResult(GetName(path)))
                {
                    yield return $"{tab}[{GetName(path)}]";

                    foreach (var file in ProcessPath(path, tab + "\t"))
                    {
                        yield return file;
                    }
                }
            }
        }

        private static string GetName(string path)
        {
            return path.Split("\\").Last();
        }

        private static bool GetFilteredResult(string value)
        {
            return (_filter != null && _filter(value)) || _filter == null;
        }
    }
}