namespace Auth.Events;

public interface IEvent
{
    public string Topic { get; }
}