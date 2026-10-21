using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitSharp.Buffer.Configurations;

namespace RabbitSharp.Buffer.Subscriber;

public interface IIntegrationEventBaseSubscriber
{
    public Task<(IChannel, AsyncEventingBasicConsumer)> Subscribe();
}

[ExcludeFromCodeCoverage]
public sealed class IntegrationEventBaseSubscriber(MessageQueueConnectionFactory messageQueueConnectionBuilder)
    : IIntegrationEventBaseSubscriber
{
    public async Task<(IChannel, AsyncEventingBasicConsumer)> Subscribe()
    {
        try
        {
            IChannel channel = await messageQueueConnectionBuilder.Channel;
            AsyncEventingBasicConsumer consumer = new(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                return Task.CompletedTask;
            };
            return (channel, consumer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

