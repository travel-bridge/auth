using Auth.Application.Emails;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Auth.Infrastructure.Emails;

public class EmailSender : IEmailSender
{
    private readonly EmailsOptions _options;
    private readonly SmtpClient _smtpClient;

    public EmailSender(IOptions<EmailsOptions> options)
    {
        _options = options.Value;
        
        var smtpClient = new SmtpClient();
        smtpClient.Connect(_options.Host, _options.Port, _options.EnableTls
            ? SecureSocketOptions.StartTls
            : SecureSocketOptions.Auto);
        if (_options.EnableAuthentication)
            smtpClient.Authenticate(_options.UserName, _options.Password);
        _smtpClient = smtpClient;
    }
    
    public async Task SendAsync(
        IEnumerable<string> to,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_options.From));
        foreach (var x in to)
            message.To.Add(MailboxAddress.Parse(x));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = body };

        await _smtpClient.SendAsync(message, cancellationToken);
    }

    public void Dispose()
    {
        _smtpClient.Disconnect(true);
        _smtpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}