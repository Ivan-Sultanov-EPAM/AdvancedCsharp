namespace FileObserverTask1
{
    public interface IFileSystem
    {
        string[] GetFiles(string path);
        string[] GetDirectories(string path);
    }
}