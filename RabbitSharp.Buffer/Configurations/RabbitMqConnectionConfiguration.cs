using System.ComponentModel.DataAnnotations;

namespace RabbitSharp.Buffer.Configurations;

public sealed class RabbitMqConnectionConfiguration
{
    [Required] public string HostName { get; set; } = string.Empty;

    [Required] public string UserName { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    [Required] public string Port { get; set; } = string.Empty;

    [Required] public string VirtualHost { get; set; } = string.Empty;

    [Required] public List<ExchangeMetadata> ExchangeMetadata { get; set; } = new();
}

public sealed class ExchangeMetadata
{
    [Required] public string ExchangeName { get; set; } = string.Empty;

    [Required] public string Type { get; set; } = string.Empty;

    [Required] public List<Queue> Queues { get; set; } = new();
}

public sealed class Queue
{
    [Required] public string QueueName { get; set; } = string.Empty;

    [Required] public string RoutingKey { get; set; } = string.Empty;

    [Required] public string BindingKey { get; set; } = string.Empty;
}

