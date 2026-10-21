using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RabbitSharp.Buffer.Attributes;

namespace RabbitSharp.Buffer.IntegrationEvents;

[ExcludeFromCodeCoverage]
[IntegrationEvent(exchangeName: "arm.direct.rabbitsharp",
    routingKey: "arm.direct.rabbitsharp.bills",
    bindingKey: "arm.direct.rabbitsharp.bills")]
public sealed record NewBillIntegrationEvent(
    [property: Required] string BillId,
    [property: Required] string OrderDateTime) : IIntegrationEvent;