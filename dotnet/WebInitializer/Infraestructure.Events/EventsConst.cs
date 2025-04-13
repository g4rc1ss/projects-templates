namespace Infraestructure.Events;

public class EventsConst
{
#if (UseAzServiceBus || UseRabbitMQ)
#else
    public const string CONSUMER_NAME = "Events.ConsumerService";
#endif
}