using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitSharp.Publisher.Configurations;

namespace RabbitSharp.Publisher.Endpoints;

public static class IntegrationEventEndpoints
{
    public static void MapIntegrationEventEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost(pattern: "/post-new-method",
            handler: async (NewBill newBill, IOptions<RabbitMQConnectionFactory> options) =>
            {
                CancellationTokenSource cancellationTokenSource = new();
                cancellationTokenSource.CancelAfter(delay: TimeSpan.FromMinutes(minutes: 5));
                NewBill newBill2 = newBill with
                {
                    OrderId = Guid.NewGuid().ToString()
                };

                ConnectionFactory connectionFactory = new()
                {
                    HostName = options.Value.HostName,
                    UserName = options.Value.UserName,
                    Password = options.Value.Password,
                    Port = int.Parse(options.Value.Port),
                    VirtualHost = options.Value.VirtualHost
                };

                await using IConnection connection =
                    await connectionFactory.CreateConnectionAsync(cancellationToken: cancellationTokenSource.Token);

                await using IChannel channel =
                    await connection.CreateChannelAsync(cancellationToken: cancellationTokenSource.Token);

                await channel.QueueDeclareAsync(
                    queue: "samplequeue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: new Dictionary<string, object?>()
                    {
                        {
                            "x-queue-type", "quorum"
                        }
                    },
                    cancellationToken: cancellationTokenSource.Token);

                string message = JsonSerializer.Serialize(value: newBill2);
                byte[] body = Encoding.UTF8.GetBytes(s: message);

                await channel.BasicPublishAsync(exchange: string.Empty,
                    routingKey: "samplequeue",
                    body: body,
                    cancellationToken: cancellationTokenSource.Token);
            });
    }
}

public record NewBill(string OrderId);