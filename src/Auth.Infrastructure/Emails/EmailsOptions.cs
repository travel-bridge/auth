using System.ComponentModel.DataAnnotations;

namespace Auth.Infrastructure.Emails;

public class EmailsOptions
{
    public const string SectionKey = "Emails";

    [Required]
    public string From { get; set; } = null!;

    [Required]
    public string Host { get; set; } = null!;

    [Required]
    public int Port { get; set; }

    [Required]
    public bool EnableAuthentication { get; set; }
    
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public bool EnableTls { get; set; }
}