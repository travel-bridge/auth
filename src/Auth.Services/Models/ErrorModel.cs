using System.ComponentModel.DataAnnotations;

namespace Auth.Services.Models;

// TODO: Использовать во всех View("Error")
public class ErrorModel
{
    [Required]
    public string Message { get; set; } = "Something went wrong";
}