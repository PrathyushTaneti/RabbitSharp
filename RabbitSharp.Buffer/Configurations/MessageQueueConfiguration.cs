using Microsoft.Extensions.DependencyInjection;
using RabbitSharp.Buffer.BackgroundJobs;
using RabbitSharp.Buffer.BasePublisher;
using RabbitSharp.Buffer.BaseSubscriber;

namespace RabbitSharp.Buffer.Configurations;

public static class MessageQueueConfiguration
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddMessageQueue()
        {
            serviceCollection.AddSingleton<MessageQueueConnectionBuilder>();
            serviceCollection.AddHostedService<MessageQueueConnectionWorker>();
            serviceCollection.AddScoped<IIntegrationEventBasePublisher, IntegrationEventBasePublisher>();
            serviceCollection.AddScoped<IIntegrationEventBaseSubscriber, IntegrationEventBaseSubscriber>();
            return serviceCollection;
        }
    }
}