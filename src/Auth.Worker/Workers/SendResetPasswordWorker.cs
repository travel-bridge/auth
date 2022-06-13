using Auth.Application.Emails;
using Auth.Application.Events;

namespace Auth.Worker.Workers;

public class SendResetPasswordWorker : WorkerBase
{
    private const string ConsumerGroupId = "send-reset-password-consumer-group";

    private readonly IEmailSender _emailSender;
    private readonly IEventConsumerFactory _eventConsumerFactory;

    public SendResetPasswordWorker(
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
        using var eventConsumer = _eventConsumerFactory.Subscribe(Topics.ResetPassword, ConsumerGroupId);
        
        do
        {
            await ExecuteSafelyAsync(
                async () =>
                {
                    await eventConsumer.ConsumeAndHandleAsync<ResetPasswordEvent>(
                        async @event => await SendEmailAsync(@event, _emailSender, stoppingToken),
                        stoppingToken);
                },
                stoppingToken);
        }
        while (!stoppingToken.IsCancellationRequested);
    }

    private static async Task SendEmailAsync(
        ResetPasswordEvent @event,
        IEmailSender emailSender,
        CancellationToken stoppingToken)
    {
        var to = new[] { @event.Email };
        var subject = "Reset password";
        var body = $"Reset password: {@event.CallbackUrl}";
        
        await emailSender.SendAsync(to, subject, body, stoppingToken);
    }
    
    public override void Dispose()
    {
        base.Dispose();
        _emailSender.Dispose();
        GC.SuppressFinalize(this);
    }
}