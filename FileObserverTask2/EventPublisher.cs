namespace FileObserverTask2
{
    public delegate void EventHandler(string value);

    public class EventPublisher
    {
        private string _value;
        public event EventHandler EventHandler;

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