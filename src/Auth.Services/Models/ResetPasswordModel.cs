using System.ComponentModel.DataAnnotations;

namespace Auth.Services.Models;

public class ResetPasswordModel
{
    [Required]
    [StringLength(256)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
    
    [Required]
    public string ReturnUrl { get; set; } = default!;

    [Required]
    public string UserId { get; set; } = default!;
    
    [Required]
    public string Code { get; set; } = default!;
}