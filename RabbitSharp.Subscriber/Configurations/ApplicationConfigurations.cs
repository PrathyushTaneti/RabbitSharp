using RabbitSharp.Buffer.Configurations;

namespace RabbitSharp.Subscriber.Configurations;

public static class ApplicationConfigurations
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddConfigurations()
        {
            services.AddHostedService<Worker>()
                .AddOptions<RabbitMqConnectionConfiguration>()
                .BindConfiguration("RabbitMqConnectionConfiguration")
                .ValidateDataAnnotations()
                .ValidateOnStart();
            return services;
        }

        public IServiceCollection ConfigureHostOptions()
        {
            services.Configure<HostOptions>(options =>
            {
                options.ServicesStartConcurrently = true;
                options.ServicesStopConcurrently = true;
                options.ShutdownTimeout = TimeSpan.FromMinutes(5);
                options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost;
            });
            return services;
        }
    }

    extension(ILoggingBuilder loggingBuilder)
    {
        public ILoggingBuilder ConfigureConsoleLogging()
        {
            loggingBuilder.AddSimpleConsole(options =>
            {
                options.ColorBehavior =
                    Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
                options.TimestampFormat = "[yyyy/MM/dd HH:mm:ss.fff UTC] ";
                options.SingleLine = true;
                options.IncludeScopes = false;
            });
            return loggingBuilder;
        }
    }
}
