using Auth.Application.IntegrationEvents;

namespace Auth.Worker.Workers;

public class SendResetPasswordWorker : WorkerBase
{
    private const string ConsumerGroupId = "send-reset-password-consumer-group";
    
    private readonly IEventConsumerFactory _eventConsumerFactory;

    public SendResetPasswordWorker(
        IEventConsumerFactory eventConsumerFactory,
        ILogger<WorkerBase> logger) : base(logger)
    {
        _eventConsumerFactory = eventConsumerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var eventConsumer = _eventConsumerFactory.Subscribe(Topics.ResetPassword, ConsumerGroupId);
        
        do
        {
            await ExecuteSafelyAsync(
                async () =>
                {
                    await eventConsumer.ConsumeAndHandleAsync<ResetPasswordIntegrationEvent>(
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