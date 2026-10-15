using System.ComponentModel.DataAnnotations;

namespace RabbitSharp.Subscriber.Configurations;

public sealed class RabbitMQConnectionFactory
{
    [Required] public string HostName { get; set; }
    [Required] public string UserName { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Port { get; set; }
    [Required] public string VirtualHost { get; set; }
}