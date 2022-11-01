using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileObserverTask2
{
    public class FileSystemVisitor
    {
        private readonly string _path;
        public delegate bool Filter(string value);
        private readonly Filter _filter;
        public EventPublisher _publisher = new EventPublisher();

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
            _publisher.EventHandler += x =>
            {
                Console.WriteLine($"{Environment.NewLine}*** {x} ***");
            };

            _publisher.SendMessage = "Search has started";

            Console.WriteLine($"Search in {_path}{Environment.NewLine}");

            var result = ProcessPath(_path).ToList();

            _publisher.SendMessage = "--------- Search Result ------------";

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }

            _publisher.SendMessage = "--------- Search Finished ------------";
        }

        IEnumerable<string> ProcessPath(string directory, string tab = "")
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                var fileName = GetName(file);
                _publisher.SendMessage = $"File: \"{fileName}\" Found";

                if (GetFilteredResult(fileName))
                {
                    _publisher.SendMessage = $"File: \"{fileName}\" Included";
                    yield return $"{tab}{fileName}";
                }
                else
                {
                    _publisher.SendMessage = $"File: \"{fileName}\" excluded";
                }
            }

            foreach (var path in Directory.GetDirectories(directory))
            {
                var folder = GetName(path);
                _publisher.SendMessage = $"Folder: \"{folder}\" Found";

                if (GetFilteredResult(folder))
                {
                    _publisher.SendMessage = $"Folder: \"{folder}\" Included";
                    yield return $"{tab}{folder}";
                }
                else
                {
                    _publisher.SendMessage = $"Folder: \"{folder}\" excluded";
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