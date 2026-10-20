using System.Globalization;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitSharp.Publisher.Attributes;
using RabbitSharp.Publisher.Configurations;
using RabbitSharp.Publisher.IntegrationEvents;
using RabbitSharp.Publisher.Utilities;

namespace RabbitSharp.Publisher.Publishers;

public interface IPublisherService
{
    public Task PublishEvent<T>(T eventMessage, CancellationToken cancellationToken = default) where T :
        IIntegrationEvent;
}

public class PublisherService(MessageQueueConnectionBuilder connectionBuilder) : IPublisherService
{
    public async Task PublishEvent<T>(T eventMessage, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        Type type = typeof(T);
        if (!typeof(IIntegrationEvent).IsAssignableFrom(type))
        {
            throw new InvalidOperationException("Invalid event");
        }

        IntegrationEventAttribute? integrationEventMetadata =
            eventMessage.GetMetaData();

        if (string.IsNullOrEmpty(integrationEventMetadata?.ExchangeName) ||
            string.IsNullOrEmpty(integrationEventMetadata.RoutingKey) ||
            string.IsNullOrEmpty(integrationEventMetadata.BindingKey))
        {
            throw new ArgumentException();
        }

        string message = JsonSerializer.Serialize(value: eventMessage);
        byte[] body = Encoding.UTF8.GetBytes(s: message);

        await (await connectionBuilder.Channel)
            .BasicPublishAsync(
                exchange: integrationEventMetadata.ExchangeName,
                routingKey: integrationEventMetadata.RoutingKey,
                mandatory: true,
                basicProperties: new BasicProperties()
                {
                    Persistent = true
                },
                body: body,
                cancellationToken: cancellationToken);
    }
}