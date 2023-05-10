using Flurl.Http.Configuration;
using Jazz.Application.Configuration;
using Jazz.Core.EntityFramework;
using Jazz.Covenant.Application.Configuration;
using Jazz.Covenant.Application.RestApi;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Jazz.Covenant.Service.BpoService;
using Jazz.Covenant.Service.CacheMemory;
using Jazz.Covenant.Service.CreateEndoserAdapter;
using Jazz.Covenant.Service.PsaService;
using Jazz.Covenant.Service.PsaService.Autentication;
using Jazz.Covenant.Service.TokenJwtValidated;
using Jazz.Covenant.ServiceHost.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;


try
{
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;

    #region [ Configure Serilog Options ]

    builder.Host.UseSerilog((ctx, lc) =>
                            {
                                lc.ReadFrom.Configuration(builder.Configuration);
                                lc.Enrich.WithProperty("Service", new ServiceInfo());
                            });

    #endregion

    #region [ Configure Services ]

    builder.Services
           .AddSwagger()
           .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
           .AddDbContext<CovenantDbContext>(options => options.UseSqlServer("name=ConnectionStrings:SqlServer",
                                                                     b => b.MigrationsAssembly("Jazz.Covenant.ServiceHost")))

           .AddCap(cap =>
           {
               cap.DefaultGroupName = "ms-covenant.grp";
               cap.UseEntityFramework<CovenantDbContext>();
               //cap.UseKafka(o =>
               //  {
               //    o.Servers = config.GetConnectionString("Kafka");
               //    o.MainConfig.Add("security.protocol", "SASL_SSL");
               //    o.MainConfig.Add("sasl.mechanism", "AWS_MSK_IAM");
               //  });
               cap.UseRabbitMQ(o =>
             {
                 var brokerUri = config.GetConnectionString("RabbitMQ");
                 o.ConnectionFactoryOptions = opt => opt.Uri = new Uri(brokerUri);
                 o.ExchangeName = "ms-covenant.exg";
                 
             });
               cap.UseDashboard();
           })
           .Services.AddCovenant(builder.Configuration)
           .AddHealthChecks()
           .AddCheck<CovenantDbHealthCheck>("CovenantDb");

    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
    builder.Services.AddSingleton<IPsaAutentication, PsaAutentication>();
    builder.Services.AddSingleton<ICacheMemoryService, CacheMemoryService>();
    builder.Services.AddSingleton<ITokenValidated, TokenValidated>();
    builder.Services.AddSingleton<IPsaService, PsaService>();
    builder.Services.AddSingleton<IBpoService, BpoService>();
    builder.Services.AddSingleton<ICreateEndoserAdapter, CreateEndoserAdapter>();
    builder.Services.AddScoped<IEndoserAdapterService, EndoserAdapterService>();
    builder.Services.Configure<Jazz.Covenant.Service.Settings.Bpo>(config.GetSection(nameof(Jazz.Covenant.Service.Settings.Bpo)));

    #endregion

    var app = builder.Build();

    #region [ Use Services ]

    app.UseUserNameLogging();
    app.UseValidation();

    app.MapCovenant();

    app.MapHealthChecks("/healthz");

    app.UseSwagger("Covenant");

    #endregion

    #region [ Database Migration ]

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<CovenantDbContext>();
        context.Database.Migrate();
    }

    #endregion

    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
