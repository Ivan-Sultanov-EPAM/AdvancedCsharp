using System.IO;

namespace FileObserverTask1
{
    public class FileSystem : IFileSystem
    {
        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }
    }
}