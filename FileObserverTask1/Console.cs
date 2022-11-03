namespace FileObserverTask1
{
    public class Console : IConsole
    {
        public void Write(string s)
        {
            System.Console.WriteLine(s);
        }
        public string Read()
        {
            return System.Console.ReadLine();
        }
    }
}