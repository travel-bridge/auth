namespace Auth.Application.Events;

public record ResetPasswordEvent(string Email, string Code, string CallbackUrl) : IEvent
{
    public string GetTopic() => Topics.ResetPassword;
}