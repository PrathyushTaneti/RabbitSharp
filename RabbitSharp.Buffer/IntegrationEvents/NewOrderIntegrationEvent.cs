using System.ComponentModel.DataAnnotations;
using RabbitSharp.Buffer.Attributes;

namespace RabbitSharp.Buffer.IntegrationEvents;

[IntegrationEvent("arm.direct.rabbitsharp", "arm.direct.rabbitsharp.orders", "arm.direct.rabbitsharp.orders")]
public sealed record NewOrderIntegrationEvent(
    [property: Required] string OrderId,
    [property: Required] string OrderDate) : IIntegrationEvent;