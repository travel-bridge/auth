namespace Auth.Events;

public class ResetPasswordEvent : IEvent
{
    public string Topic => Topics.ResetPassword;
    
    public string Code { get; set; } = null!;

    public string CallbackUrl { get; set; } = null!;
}