namespace Auth.Application.IntegrationEvents;

public interface IEventConsumerFactory
{
    IEventConsumer Subscribe(string topic, string groupId);
}