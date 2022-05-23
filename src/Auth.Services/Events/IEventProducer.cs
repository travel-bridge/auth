using Auth.Events;

namespace Auth.Services.Events;

public interface IEventProducer : IDisposable
{
    Task ProduceAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}