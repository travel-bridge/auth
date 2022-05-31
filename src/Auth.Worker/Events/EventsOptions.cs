using System.ComponentModel.DataAnnotations;

namespace Auth.Worker.Events;

public class EventsOptions
{
    public const string SectionKey = "Events";

    [Required]
    public string BootstrapServers { get; set; } = null!;
}