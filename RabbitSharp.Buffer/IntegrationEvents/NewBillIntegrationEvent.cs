using System.ComponentModel.DataAnnotations;
using RabbitSharp.Buffer.Attributes;

namespace RabbitSharp.Buffer.IntegrationEvents;

[IntegrationEvent("arm.direct.rabbitsharp", "arm.direct.rabbitsharp.bills", "arm.direct.rabbitsharp.bills")]
public sealed record NewBillIntegrationEvent(
    [property: Required] string BillId,
    [property: Required] string OrderDateTime) : IIntegrationEvent;