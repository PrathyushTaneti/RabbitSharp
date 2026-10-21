using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RabbitSharp.Buffer.Attributes;

namespace RabbitSharp.Buffer.IntegrationEvents;

[ExcludeFromCodeCoverage]
[IntegrationEvent(exchangeName: "arm.direct.rabbitsharp",
    routingKey: "arm.direct.rabbitsharp.orders",
    bindingKey: "arm.direct.rabbitsharp.orders")]
public sealed record NewOrderIntegrationEvent(
    [property: Required] string OrderId,
    [property: Required] string OrderDate) : IIntegrationEvent;