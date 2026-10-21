using System.Globalization;
using System.Text.Json;
using RabbitSharp.Buffer.IntegrationEvents;
using RabbitSharp.Buffer.Publisher;

namespace RabbitSharp.Publisher.Endpoints;

public static class IntegrationEventEndpoints
{
    public static void MapIntegrationEventEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost(
            pattern: "/add-new-bill",
            handler: SendNewBill);
    }

    private static async Task SendNewBill(
        IIntegrationEventBasePublisher publisherService,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        ILogger logger = loggerFactory.CreateLogger("IntegrationEventEndpoints");
        const string logPrefix = $"{nameof(IntegrationEventEndpoints)}.{nameof(SendNewBill)}";
        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.CancelAfter(delay: TimeSpan.FromMinutes(minutes: 5));

        NewBillIntegrationEvent newBill = new(
            Guid.NewGuid().ToString(),
            DateTime.UtcNow.ToString(provider: CultureInfo.CurrentCulture));

        logger.LogInformation("{prefix} Sending new bill integration event : {bill}", logPrefix,
            JsonSerializer.Serialize(newBill));

        await publisherService.PublishEvent(newBill, cancellationTokenSource.Token);
    }
}