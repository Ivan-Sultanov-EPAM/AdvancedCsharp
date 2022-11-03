namespace FileObserverTask2
{
    public delegate void MessageEventHandler(string value);

    public class EventPublisher
    {
        private string _value;
        public event MessageEventHandler EventHandler;

        public string SendMessage
        {
            set
            {
                _value = value;
                EventHandler(_value);
            }
        }
    }
}