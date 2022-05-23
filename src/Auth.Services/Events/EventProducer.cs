using System.Net;
using System.Text.Json;
using Auth.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Auth.Services.Events;

public class EventProducer : IEventProducer
{
    private readonly IProducer<Null, string> _producer;

    public EventProducer(IOptions<KafkaOptions> kafkaOptions)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaOptions.Value.BootstrapServers,
            ClientId = Dns.GetHostName()
        };

        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    }
    
    public EventProducer(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    public async Task ProduceAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var json = JsonSerializer.Serialize(@event);
        var message = new Message<Null, string> { Value = json };
        await _producer.ProduceAsync(@event.Topic, message, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Dispose();
        GC.SuppressFinalize(this);
    }
}