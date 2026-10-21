using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitSharp.Buffer.Attributes;
using RabbitSharp.Buffer.Configurations;
using RabbitSharp.Buffer.Extensions;
using RabbitSharp.Buffer.IntegrationEvents;

namespace RabbitSharp.Buffer.Publisher;

public interface IIntegrationEventBasePublisher
{
    public Task PublishEvent<T>(T eventMessage, CancellationToken cancellationToken = default) where T :
        IIntegrationEvent;
}

[ExcludeFromCodeCoverage]
public class IntegrationEventBasePublisher(
    MessageQueueConnectionFactory connectionBuilder,
    ILogger<IntegrationEventBasePublisher> logger)
    : IIntegrationEventBasePublisher
{
    public async Task PublishEvent<T>(T eventMessage, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        const string logPrefix = $"{nameof(IntegrationEventBasePublisher)}.{nameof(PublishEvent)}";

        try
        {
            Type type = typeof(T);
            logger.LogInformation("{logPrefix} Publishing Event of type : {type}", logPrefix, type);

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

            logger.LogInformation("{logPrefix} Publishing Event To Exchange : {exchange}", logPrefix,
                integrationEventMetadata.ExchangeName);

            byte[] body = JsonSerializer.SerializeToUtf8Bytes(value: eventMessage);

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
            logger.LogInformation("{logPrefix} Publishing Event Successful", logPrefix);
        }
        catch (Exception exception)
        {
            logger.LogError("{logPrefix} Exception thrown {exception}", logPrefix, exception.Message);
        }
    }
}