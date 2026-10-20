namespace RabbitSharp.Publisher.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class IntegrationEventAttribute : Attribute
{
    public IntegrationEventAttribute(string exchangeName, string routingKey, string bindingKey)
    {
        ExchangeName = exchangeName;
        RoutingKey = routingKey;
        BindingKey = bindingKey;
    }

    public string ExchangeName { get; set; }

    public string RoutingKey { get; set; }

    public string BindingKey { get; set; }
}