using RabbitSharp.Publisher.HostedServices;
using RabbitSharp.Publisher.Publishers;

namespace RabbitSharp.Publisher.Configurations;

public static class ApplicationConfigurations
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddConfigurations(IConfiguration configuration)
        {
            services
                .AddOpenApi()
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


        public IServiceCollection AddConnection()
        {
            services.AddSingleton<MessageQueueConnectionBuilder>();
            services.AddHostedService<MessageQueueConnectionWorker>();
            services.AddScoped<IPublisherService, PublisherService>();
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
                options.IncludeScopes = true;
            });
            return loggingBuilder;
        }
    }
}
