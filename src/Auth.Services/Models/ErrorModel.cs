using System.ComponentModel.DataAnnotations;

namespace Auth.Services.Models;

// TODO: Use it in all View("Error")
public class ErrorModel
{
    [Required]
    public string Message { get; set; } = "Something went wrong";
}