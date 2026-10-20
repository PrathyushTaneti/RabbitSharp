using System.Globalization;
using RabbitSharp.Publisher.IntegrationEvents;
using RabbitSharp.Publisher.Publishers;

namespace RabbitSharp.Publisher.Endpoints;

public static class IntegrationEventEndpoints
{
    public static void MapIntegrationEventEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost(
            pattern: "/add-new-bill",
            handler: SendNewBill);
    }

    private static async Task SendNewBill(IPublisherService publisherService)
    {
        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.CancelAfter(delay: TimeSpan.FromMinutes(minutes: 5));

        NewBillIntegrationEvent newBill = new(
            Guid.NewGuid().ToString(),
            DateTime.UtcNow.ToString(provider: CultureInfo.CurrentCulture));

        await publisherService.PublishEvent(newBill, cancellationTokenSource.Token);
    }
}