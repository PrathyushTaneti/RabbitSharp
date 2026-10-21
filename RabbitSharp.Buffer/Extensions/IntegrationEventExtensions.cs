using System.Reflection;
using RabbitSharp.Buffer.Attributes;
using RabbitSharp.Buffer.IntegrationEvents;

namespace RabbitSharp.Buffer.Extensions;

public static class IntegrationEventExtensions
{
    extension(IIntegrationEvent integrationEventntegrationEvent)
    {
        public IntegrationEventAttribute? GetMetaData()
        {
            Type type = integrationEventntegrationEvent.GetType();
            if (!typeof(IIntegrationEvent).IsAssignableFrom(type))
            {
                throw new ArgumentException($"Type {type.Name} must implement {nameof(IIntegrationEvent)}");
            }

            IntegrationEventAttribute? attribute = type.GetCustomAttribute<IntegrationEventAttribute>();
            return attribute;
        }
    }
}