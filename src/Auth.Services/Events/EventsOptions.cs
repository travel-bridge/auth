using System.ComponentModel.DataAnnotations;

namespace Auth.Services.Events;

public class EventsOptions
{
    public const string SectionKey = "Events";

    [Required] public string BootstrapServers { get; set; } = null!;
}