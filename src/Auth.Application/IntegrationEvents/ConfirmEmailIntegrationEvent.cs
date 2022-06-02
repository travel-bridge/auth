namespace Auth.Application.IntegrationEvents;

public class ConfirmEmailIntegrationEvent : IIntegrationEvent
{
    public string Code { get; set; } = null!;

    public string CallbackUrl { get; set; } = null!;
    
    public string GetTopic() => Topics.ConfirmEmail;
}