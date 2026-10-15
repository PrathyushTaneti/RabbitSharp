using RabbitSharp.Subscriber.Configurations;

namespace RabbitSharp.Subscriber;

internal class Program
{
    public static async Task Main(string[] args)
    {
        HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder(args);
        {
            hostApplicationBuilder.Services.AddConfigurations().ConfigureHostOptions();
            hostApplicationBuilder.Logging.ConfigureConsoleLogging();
        }

        IHost host = hostApplicationBuilder.Build();
        {
            await host.RunAsync();
        }
    }
}