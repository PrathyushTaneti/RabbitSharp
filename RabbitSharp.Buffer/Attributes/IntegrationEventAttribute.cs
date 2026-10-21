namespace RabbitSharp.Buffer.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class IntegrationEventAttribute(string exchangeName, string routingKey, string bindingKey) : Attribute
{
    public string ExchangeName { get; set; } = exchangeName;

    public string RoutingKey { get; set; } = routingKey;

    public string BindingKey { get; set; } = bindingKey;
}