using System;

namespace FileObserverTask1
{
    public class Search
    {
        private static readonly string Path = GetDownloadFolder.GetFolder();
        private static string _exit;
        private static readonly string NewLine = Environment.NewLine;
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;

        public Search(IFileSystem fileSystem, IConsole console)
        {
            _console = console;
            _fileSystem = fileSystem;
        }

        public void Process()
        {
            do
            {
                _console.Write($"Please enter the path, or press enter to search in Downloads folder:{NewLine}");
                var path = _console.Read();

                _console.Write($"Please enter the filtering string:{NewLine}");
                var filter = _console.Read();

                try
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        var visitor = new FileSystemVisitor(_fileSystem, _console, string.IsNullOrEmpty(path) ? Path : path,
                            x => x.ToLower().Contains(filter.ToLower()));

                        visitor.Search();
                    }
                    else
                    {
                        var visitor = new FileSystemVisitor(_fileSystem, _console, string.IsNullOrEmpty(path) ? Path : path);

                        visitor.Search();
                    }
                }
                catch (Exception e)
                {
                    _console.Write($"Something went wrong: {e.Message},{NewLine}please check the provided path");
                }

                _console.Write($"{NewLine}- press enter to repeat search" +
                                   $"{NewLine}- enter \"q\" to quit{NewLine}");

                _exit = _console.Read();
            } while (_exit?.ToLower() != "q");
        }
    }
}