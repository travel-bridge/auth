namespace Auth.Application.IntegrationEvents;

public record ResetPasswordIntegrationEvent(string Code, string CallbackUrl) : IIntegrationEvent
{
    public string GetTopic() => Topics.ResetPassword;
}