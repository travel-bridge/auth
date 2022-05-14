using System.ComponentModel.DataAnnotations;

namespace Auth.Services.Models;

public class ForgotPasswordModel
{
    [Required]
    [StringLength(256)]
    [EmailAddress]
    public string Email { get; set; } = default!;
    
    [Required]
    public string ReturnUrl { get; set; } = default!;
}