namespace Auth.Application.Emails;

public interface IEmailSender : IDisposable
{
    Task SendAsync(
        IEnumerable<string> to,
        string subject,
        string body,
        CancellationToken cancellationToken = default);
}