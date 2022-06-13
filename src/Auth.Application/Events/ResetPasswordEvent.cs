namespace Auth.Application.Events;

public record ResetPasswordEvent(string Code, string CallbackUrl) : IEvent
{
    public string GetTopic() => Topics.ResetPassword;
}