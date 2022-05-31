namespace Auth.Events;

public class ResetPasswordEvent : IEvent
{
    public string Code { get; set; } = null!;

    public string CallbackUrl { get; set; } = null!;
    
    public string GetTopic() => Topics.ResetPassword;
}