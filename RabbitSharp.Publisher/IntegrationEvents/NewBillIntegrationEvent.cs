
using System.ComponentModel.DataAnnotations;
using RabbitSharp.Publisher.Attributes;

namespace RabbitSharp.Publisher.IntegrationEvents;

[IntegrationEvent("arm.direct.rabbitsharp", "arm.direct.rabbitsharp.bills", "arm.direct.rabbitsharp.bills")]
public sealed record NewBillIntegrationEvent(
    [property: Required] string BillId,
    [property: Required] string OrderDateTime) : IIntegrationEvent;