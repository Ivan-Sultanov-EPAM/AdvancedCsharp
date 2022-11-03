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
        private static Filter _filter;
        private readonly EventPublisher _publisher = new EventPublisher();
        private bool _stopProcess;

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

        public void Visit()
        {
            _publisher.EventHandler += x =>
            {
                Console.WriteLine($"{Environment.NewLine}*** {x} ***");
            };

            _publisher.ActionHandler += () =>
            {
                Console.WriteLine(
                    $"Proceed: press enter{Environment.NewLine}" +
                    $"Include: enter 1{Environment.NewLine}" +
                    $"Exclude: enter 2{Environment.NewLine}" +
                    $"Stop search: enter 3{Environment.NewLine}"
                    );

                var result = Console.ReadLine();

                return result switch
                {
                    "1" => ActionsEnum.Include,
                    "2" => ActionsEnum.Exclude,
                    "3" => ActionsEnum.StopSearch,
                    _ => ActionsEnum.Proceed,
                };
            };

            _publisher.SendMessage = "Search has started";

            Console.WriteLine($"Search in {_path}{Environment.NewLine}");

            var result = ProcessPath(_path).ToList();

            _publisher.SendMessage = "--------- Search Result ------------";

            result.ForEach(item => Console.WriteLine(item));

            if (result.Count == 0)
            {
                Console.WriteLine("\t    No results");
            }

            _publisher.SendMessage = "-------- Search Finished -----------";
        }

        private IEnumerable<string> ProcessPath(string directory, string tab = "")
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                if (_stopProcess) break;

                var fileName = GetName(file);

                _publisher.SendMessage = GetFilteredResult(fileName) ?
                    $"File: \"{fileName}\" found and will be included" : $"File: \"{fileName}\"found and will be filtered";

                var actionResult = _publisher.ActionRequest();

                var include = true;
                var includeFiltered = false;

                switch (actionResult)
                {
                    case ActionsEnum.Include:
                        includeFiltered = true;
                        break;
                    case ActionsEnum.Exclude:
                        include = false;
                        break;
                    case ActionsEnum.StopSearch:
                        _stopProcess = true;
                        break;
                    case ActionsEnum.Proceed:
                        break;
                }

                if (_stopProcess) break;

                if ((GetFilteredResult(fileName) && include) || includeFiltered)
                {
                    _publisher.SendMessage = $"File: \"{fileName}\" included";
                    yield return $"{tab}{fileName}";
                }
                else
                {
                    _publisher.SendMessage = $"File: \"{fileName}\" excluded";
                }
            }

            foreach (var path in Directory.GetDirectories(directory))
            {
                if (_stopProcess) break;

                var folderName = GetName(path);

                _publisher.SendMessage = GetFilteredResult(folderName) ?
                    $"File: \"{folderName}\" found and will be included" : $"File: \"{folderName}\"found and will be filtered";

                var actionResult = _publisher.ActionRequest();

                var include = true;
                var includeFiltered = false;

                switch (actionResult)
                {
                    case ActionsEnum.Include:
                        includeFiltered = true;
                        break;
                    case ActionsEnum.Exclude:
                        include = false;
                        break;
                    case ActionsEnum.StopSearch:
                        _stopProcess = true;
                        break;
                    case ActionsEnum.Proceed:
                        break;
                }

                if (_stopProcess) break;

                if ((GetFilteredResult(folderName) && include) || includeFiltered)
                {
                    _publisher.SendMessage = $"Folder: \"{folderName}\" included";
                    yield return $"{tab}[{folderName}]";

                    foreach (var file in ProcessPath(path, tab + "\t"))
                    {
                        yield return file;
                    }
                }
                else
                {
                    _publisher.SendMessage = $"Folder: \"{folderName}\" excluded";
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