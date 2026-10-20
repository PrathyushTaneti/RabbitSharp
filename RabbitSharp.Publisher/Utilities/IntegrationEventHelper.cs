using System.Reflection;
using RabbitSharp.Publisher.Attributes;
using RabbitSharp.Publisher.IntegrationEvents;

namespace RabbitSharp.Publisher.Utilities;

public static class IntegrationEventHelper
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