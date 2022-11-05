using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileObserverTask2
{
    internal class FileSystemVisitor
    {
        private readonly string _path;
        public delegate bool Filter(string value);
        private static Filter _filter;
        private bool _stopProcess;
        private static readonly string NewLine = Environment.NewLine;
        public delegate void MessageEventHandler(string value);
        public delegate ActionsEnum ActionEventHandler();
        public event MessageEventHandler EventHandler;
        public event ActionEventHandler ActionHandler;

        public FileSystemVisitor(string path)
        {
            _path = path;
            _filter = null;
        }

        public FileSystemVisitor(string path, Filter filter) : this(path)
        {
            _filter = filter;
        }

        public void Search()
        {
            SendMessage($"Search has started in {_path}{NewLine}");

            var result = ProcessPath(_path).ToList();

            SendMessage($"{NewLine}--------- Search Result ------------{NewLine}");

            result.ForEach(item => SendMessage(item));

            if (result.Count == 0)
            {
                SendMessage($"\t    No results{NewLine}");
            }

            SendMessage($"{NewLine}-------- Search Finished -----------");
        }

        private IEnumerable<string> ProcessPath(string directory, string tab = "")
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                if (_stopProcess) break;

                var fileName = GetName(file);

                var message = GetFilteredResult(fileName) ?
                    $"File found: \"{fileName}\"{NewLine}and will be included{NewLine}"
                    : $"File found: \"{fileName}\"{NewLine}and will be filtered{NewLine}";

                SendMessage(message);

                var actionResult = ActionRequest();

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
                    SendMessage($"File: \"{fileName}\"{NewLine}included{NewLine}");
                    yield return $"{tab}{fileName}";
                }
                else
                {
                    SendMessage($"File: \"{fileName}\"{NewLine}excluded{NewLine}");
                }
            }

            foreach (var path in Directory.GetDirectories(directory))
            {
                if (_stopProcess) break;

                var folderName = GetName(path);

                var message = GetFilteredResult(folderName) ?
                    $"Folder found: \"{folderName}\"{NewLine}and will be included{NewLine}"
                    : $"Folder found: \"{folderName}\"{NewLine}and will be filtered{NewLine}";

                SendMessage(message);

                var actionResult = ActionRequest();

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
                    SendMessage($"Folder: \"{folderName}\"{NewLine}included{NewLine}");
                    yield return $"{tab}[{folderName}]";

                    foreach (var file in ProcessPath(path, tab + "\t"))
                    {
                        yield return file;
                    }
                }
                else
                {
                    SendMessage($"Folder: \"{folderName}\"{NewLine}excluded{NewLine}");
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

        private ActionsEnum ActionRequest()
        {
            return ActionHandler?.Invoke() ?? ActionsEnum.Proceed;
        }

        private void SendMessage(string value)
        {
            EventHandler?.Invoke(value);
        }
    }
}