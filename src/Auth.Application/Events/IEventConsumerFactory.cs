namespace Auth.Application.Events;

public interface IEventConsumerFactory
{
    IEventConsumer Subscribe(string topic, string groupId);
}