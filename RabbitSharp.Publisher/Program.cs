using RabbitSharp.Publisher.Configurations;
using RabbitSharp.Publisher.Endpoints;

namespace RabbitSharp.Publisher;

internal sealed class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args: args);
        {
            builder.Services
                .AddConfigurations(builder.Configuration)
                .ConfigureHostOptions()
                .AddConnection();
            builder.Logging.ConfigureConsoleLogging();
        }

        WebApplication webApplication = builder.Build();
        {
            webApplication.MapOpenApi();
            webApplication.UseHttpsRedirection();
            webApplication.MapIntegrationEventEndpoints();
            await webApplication.RunAsync();
        }
    }
}

