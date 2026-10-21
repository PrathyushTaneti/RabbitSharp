using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using RabbitSharp.Buffer.Publisher;
using RabbitSharp.Buffer.Subscriber;

namespace RabbitSharp.Buffer.Configurations;

[ExcludeFromCodeCoverage]
public static class MessageQueueConfigurationSetup
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddMessageQueue()
        {
            serviceCollection.AddSingleton<MessageQueueConnectionFactory>();
            serviceCollection.AddHostedService<HostedServices.MessageQueueConnectionBuilder>();
            serviceCollection.AddScoped<IIntegrationEventBasePublisher, IntegrationEventBasePublisher>();
            serviceCollection.AddScoped<IIntegrationEventBaseSubscriber, IntegrationEventBaseSubscriber>();
            return serviceCollection;
        }
    }
}