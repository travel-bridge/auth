using Auth.Application.Events;
using Auth.Infrastructure.Database;
using Auth.Infrastructure.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AuthDatabase")
            ?? throw new InvalidOperationException("Connection string is not configured.");

        services.AddDbContext<DataContext>(
            x => x.UseNpgsql(
                connectionString,
                options => options.EnableRetryOnFailure(
                    3,
                    TimeSpan.FromSeconds(10),
                    null)));
        
        return services;
    }

    public static IServiceCollection AddEvents(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<EventsOptions>()
            .Bind(configuration.GetSection(EventsOptions.SectionKey))
            .ValidateDataAnnotations();

        services.AddSingleton<IEventProducer, EventProducer>();
        services.AddSingleton<IEventConsumerFactory, EventConsumerFactory>();
        
        return services;
    }
}