using RabbitSharp.Buffer.Configurations;
using RabbitSharp.Subscriber.Configurations;

namespace RabbitSharp.Subscriber;

internal class Program
{
    public static async Task Main(string[] args) => await CreateApplicationHost(args).RunAsync();

    private static IHost CreateApplicationHost(string[] args)
    {
        HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder(args);
        {
            hostApplicationBuilder.Services
                .AddConfigurations()
                .ConfigureHostOptions()
                .AddMessageQueue();
            hostApplicationBuilder.Logging
                .ConfigureConsoleLogging();
        }
        return hostApplicationBuilder.Build();
    }
}