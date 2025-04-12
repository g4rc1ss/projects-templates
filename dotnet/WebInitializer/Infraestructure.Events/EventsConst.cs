namespace Infraestructure.Events;

public class EventsConst
{
#if (UseMemoryEvents)
    public const string CONSUMER_NAME = "Events.ConsumerService";
#endif
}