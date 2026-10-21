using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitSharp.Buffer.Configurations;

namespace RabbitSharp.Buffer.BaseSubscriber;

public sealed class IntegrationEventBaseSubscriber(MessageQueueConnectionBuilder messageQueueConnectionBuilder)
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

public interface IIntegrationEventBaseSubscriber
{
    public Task<(IChannel, AsyncEventingBasicConsumer)> Subscribe();
}