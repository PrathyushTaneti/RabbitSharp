using System.ComponentModel.DataAnnotations;
using RabbitSharp.Publisher.Attributes;

namespace RabbitSharp.Publisher.IntegrationEvents;

[IntegrationEvent("arm.direct.rabbitsharp", "arm.direct.rabbitsharp.orders", "arm.direct.rabbitsharp.orders")]
public sealed record NewOrderIntegrationEvent(
    [property: Required] string OrderId,
    [property: Required] string OrderDate) : IIntegrationEvent;