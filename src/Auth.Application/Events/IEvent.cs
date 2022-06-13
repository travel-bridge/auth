namespace Auth.Application.Events;

public interface IEvent
{
    string GetTopic();
}