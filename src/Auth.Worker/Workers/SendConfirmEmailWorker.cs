using Auth.Application.Emails;
using Auth.Application.Events;

namespace Auth.Worker.Workers;

public class SendConfirmEmailWorker : WorkerBase
{
    private const string ConsumerGroupId = "send-confirm-email-consumer-group";

    private readonly IEmailSender _emailSender;
    private readonly IEventConsumerFactory _eventConsumerFactory;

    public SendConfirmEmailWorker(
        IEmailSender emailSender,
        IEventConsumerFactory eventConsumerFactory,
        ILogger<WorkerBase> logger)
        : base(logger)
    {
        _emailSender = emailSender;
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
                    await eventConsumer.ConsumeAndHandleAsync<ConfirmEmailEvent>(
                        async @event => await SendEmailAsync(@event, _emailSender, stoppingToken),
                        stoppingToken);
                },
                stoppingToken);
        }
        while (!stoppingToken.IsCancellationRequested);
    }
    
    private static async Task SendEmailAsync(
        ConfirmEmailEvent @event,
        IEmailSender emailSender,
        CancellationToken stoppingToken)
    {
        var to = new[] { @event.Email };
        var subject = "Confirm email";
        var body = $"Confirm email: {@event.CallbackUrl}";
        
        await emailSender.SendAsync(to, subject, body, stoppingToken);
    }

    public override void Dispose()
    {
        base.Dispose();
        _emailSender.Dispose();
        GC.SuppressFinalize(this);
    }
}