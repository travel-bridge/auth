namespace Auth.Application.Events;

public record ConfirmEmailEvent(string Code, string CallbackUrl) : IEvent
{
    public string GetTopic() => Topics.ConfirmEmail;
}