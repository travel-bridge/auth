namespace Auth.Events;

public class ConfirmEmailEvent : IEvent
{
    public string Topic => Topics.ConfirmEmail;

    public string Code { get; set; } = null!;

    public string CallbackUrl { get; set; } = null!;
}