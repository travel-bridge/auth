using System.ComponentModel.DataAnnotations;

namespace Auth.Services.Models;

public class RegisterModel
{
    [Required]
    [StringLength(256)]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(256)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required]
    public string ReturnUrl { get; set; } = default!;
}