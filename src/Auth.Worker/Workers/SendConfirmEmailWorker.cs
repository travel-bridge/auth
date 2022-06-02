using Auth.Application.IntegrationEvents;

namespace Auth.Worker.Workers;

public class SendConfirmEmailWorker : WorkerBase
{
    private const string ConsumerGroupId = "send-confirm-email-consumer-group";
    
    private readonly IEventConsumerFactory _eventConsumerFactory;

    public SendConfirmEmailWorker(
        IEventConsumerFactory eventConsumerFactory,
        ILogger<WorkerBase> logger)
        : base(logger)
    {
        _eventConsumerFactory = eventConsumerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var eventConsumer = _eventConsumerFactory.Subscribe(Topics.ConfirmEmail, ConsumerGroupId);

        do
        {
            await ExecuteSafelyAsync(
                async () =>
                {
                    await eventConsumer.ConsumeAndHandleAsync<ConfirmEmailIntegrationEvent>(
                        @event =>
                        {
                            // TODO: Implement email sending
                            return Task.CompletedTask;
                        },
                        stoppingToken);
                },
                stoppingToken);
        }
        while (!stoppingToken.IsCancellationRequested);
    }
}