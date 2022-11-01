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
        private readonly Filter _filter;

        public FileSystemVisitor(string path)
        {
            _path = path;
        }

        public FileSystemVisitor(string path, Filter filter)
        {
            _path = path;
            _filter = filter;
        }

        public void Visit()
        {
            Console.WriteLine($"Search in {_path}{Environment.NewLine}");

            foreach (var item in ProcessPath(_path))
            {
                Console.WriteLine(item);
            }
        }

        IEnumerable<string> ProcessPath(string directory, string tab = "")
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
                    yield return $"{tab}{GetName(path)}";
                }

                foreach (var file in ProcessPath(path, tab + "\t"))
                {
                    yield return file;
                }
            }
        }

        string GetName(string path)
        {
            var splitPath = path.Split("\\");
            return splitPath.Last();
        }

        bool GetFilteredResult(string value)
        {
            if (_filter != null && _filter(value))
                return true;

            if (_filter == null) return true;

            return false;
        }
    }
}