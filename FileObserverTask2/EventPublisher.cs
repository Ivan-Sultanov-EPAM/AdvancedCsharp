namespace FileObserverTask2
{
    public delegate void MessageEventHandler(string value);
    public delegate ActionsEnum ActionEventHandler();

    public class EventPublisher
    {
        private string _value;
        public event MessageEventHandler EventHandler;
        public event ActionEventHandler ActionHandler;

        public string SendMessage
        {
            set
            {
                _value = value;
                EventHandler(_value);
            }
        }

        public ActionsEnum ActionRequest()
        {
            return ActionHandler();
        }
    }
}