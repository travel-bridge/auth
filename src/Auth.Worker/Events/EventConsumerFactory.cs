using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Auth.Worker.Events;

public class EventConsumerFactory : IEventConsumerFactory
{
    private readonly KafkaOptions _kafkaOptions;

    public EventConsumerFactory(IOptions<KafkaOptions> kafkaOptions)
    {
        _kafkaOptions = kafkaOptions.Value;
    }
    
    public IEventConsumer Subscribe(string topic, string groupId)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
        
        var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);
        var eventConsumer = new EventConsumer(consumer);
        
        return eventConsumer;
    }
}