using System.Data;
using System.Net.NetworkInformation;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Core;
using Jazz.Core.EntityFramework;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.Filter;
using Jazz.Covenant.Application.Filter.CovenantFilter;
using Jazz.Covenant.Application.MessageConsumers;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Application.Services;
using Jazz.EntityFramework;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Jazz.Covenant.Application.Configuration;

public static class ConfigureServicesExtensions
{
    public static IServiceCollection AddCovenant(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssemblyContaining<FindMarginCreditCardPayload>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetMarginLoanHandler).Assembly));

        services.AddScoped<IDbConnection>(_ => new SqlConnection(configuration.GetConnectionString("SqlServer")));
        services.AddScoped<EntityFrameworkUnitOfWork<CovenantDbContext>>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EntityFrameworkUnitOfWork<CovenantDbContext>>());
        services.AddScoped<IAggregateSavedHandler>(sp => sp.GetRequiredService<EntityFrameworkUnitOfWork<CovenantDbContext>>());

        services.AddScoped<CancelMarginReservedConsumer>();
        services.AddScoped<RegistedNotMarginConsumer>();
        services.AddScoped<EndosamentMarginConsumer>();
        services.AddScoped<MarginReservedConsumer>();
        services.AddScoped<CancelMarginEndosermentConsumer>();

        // Ao registrar os repositórios, precisamos adicionar os serviços que assinam o evento AggregateRootSaved
        services.AddScoped<ICovenantRepository, CovenantRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<CovenantDbContext>();
                var handlers = sp.GetServices<IAggregateSavedHandler>().ToList();
                var repo = new CovenantRepository(dbContext);
                handlers.ForEach(
                    h => repo.AggregateRootSaved += (_, args) => h.HandleAggregateSaved(args.AggregateRoot));
                return repo;
            });

        services.AddScoped<IEndoserAdapterService, EndoserAdapterService>();
        services.AddScoped<IFilterManager, CovenantManagerFilter>();

        return services;
    }
}

public class CovenantDbHealthCheck : IHealthCheck
{
    private readonly IDbConnection _connection;

    public CovenantDbHealthCheck(IDbConnection connection)
    {
        _connection = connection;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using (var con = _connection)
            {
                con.Open();

                return Task.FromResult(
                    HealthCheckResult.Healthy("Database connection for Covenant context is healthy."));
            }
        }
        catch (Exception ex)
        {
            return Task.FromResult(
                new HealthCheckResult(
                    context.Registration.FailureStatus,
                    "Database connection for Covenant context is unhealthy.",
                    ex)
            );
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
