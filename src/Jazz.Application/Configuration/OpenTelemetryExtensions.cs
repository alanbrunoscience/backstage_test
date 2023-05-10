using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


namespace Jazz.Application.Configuration;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services)
    {
        var (serviceName, serviceVersion) = new ServiceInfo();
        
        services.AddOpenTelemetryTracing(tracerProviderBuilder =>
                                         {
                                             tracerProviderBuilder
                                                 .AddConsoleExporter()
                                                 .AddSource(serviceName)
                                                 .SetResourceBuilder(ResourceBuilder.CreateDefault()
                                                                                    .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
                                                 .AddHttpClientInstrumentation()
                                                 .AddAspNetCoreInstrumentation()
                                                 .AddSqlClientInstrumentation();
                                         });

        return services;
    }
}
