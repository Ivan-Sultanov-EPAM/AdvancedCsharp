using System;
using System.Collections.Generic;
using System.Linq;

namespace FileObserverTask1
{
    public class FileSystemVisitor
    {
        private readonly string _path;
        public delegate bool Filter(string value);
        private static Filter _filter;
        private static readonly string NewLine = Environment.NewLine;
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;

        public FileSystemVisitor(IFileSystem fileSystem, IConsole console, string path)
        {
            _console = console;
            _path = path;
            _filter = null;
            _fileSystem = fileSystem;
        }

        public FileSystemVisitor(IFileSystem fileSystem, IConsole console, string path, Filter filter)
            : this(fileSystem, console, path)
        {
            _filter = filter;
        }

        public void Search()
        {
            _console.Write($"Search in {_path}{NewLine}" +
                              $"{NewLine}--------- Search Result -----------{NewLine}");
            var count = 0;

            foreach (var item in ProcessPath(_path))
            {
                _console.Write(item);
                count++;
            }

            if (count == 0)
                _console.Write("\t    No results");

            _console.Write($"{NewLine}-------- Search Finished ------------{NewLine}");
        }

        private IEnumerable<string> ProcessPath(string directory, string tab = "")
        {
            foreach (var file in _fileSystem.GetFiles(directory))
            {
                var fileName = GetName(file);

                if (GetFilteredResult(fileName))
                {
                    yield return $"{tab}{fileName}";
                }
            }

            foreach (var path in _fileSystem.GetDirectories(directory))
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