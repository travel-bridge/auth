using System.ComponentModel.DataAnnotations;

namespace Auth.Worker.Events;

public class KafkaOptions
{
    public const string SectionKey = "Kafka";

    [Required]
    public string BootstrapServers { get; set; } = null!;
}