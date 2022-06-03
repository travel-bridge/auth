namespace Auth.Application.IntegrationEvents;

public record ConfirmEmailIntegrationEvent(string Code, string CallbackUrl) : IIntegrationEvent
{
    public string GetTopic() => Topics.ConfirmEmail;
}