namespace Auth.Events;

public class ConfirmEmailEvent : IEvent
{
    public string Code { get; set; } = null!;

    public string CallbackUrl { get; set; } = null!;
    
    public string GetTopic() => Topics.ConfirmEmail;
}