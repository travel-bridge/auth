using System.ComponentModel.DataAnnotations;

namespace Auth.Services.Models;

public class LoginModel
{
    [Required]
    [StringLength(256)]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(256)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public bool RememberLogin { get; set; }

    [Required]
    public string ReturnUrl { get; set; } = default!;
}