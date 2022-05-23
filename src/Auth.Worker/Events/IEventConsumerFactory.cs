namespace Auth.Worker.Events;

public interface IEventConsumerFactory
{
    IEventConsumer Subscribe(string topic, string groupId);
}