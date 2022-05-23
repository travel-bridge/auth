using Auth.Events;

namespace Auth.Worker.Events;

public interface IEventConsumer : IDisposable
{
    Task ConsumeAndHandleAsync<TEvent>(
        Func<TEvent, Task> handle,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}