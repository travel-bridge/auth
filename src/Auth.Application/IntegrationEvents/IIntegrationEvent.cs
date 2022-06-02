namespace Auth.Application.IntegrationEvents;

public interface IIntegrationEvent
{
    string GetTopic();
}