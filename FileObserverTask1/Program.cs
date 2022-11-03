namespace FileObserverTask1
{
    internal class Program
    {
        private static void Main()
        {
            var console = new Console();
            var fileSystem = new FileSystem();
            var search = new Search(fileSystem, console);
            search.Process();
        }
    }
}
