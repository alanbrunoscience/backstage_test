using Jazz.Common;
using Jazz.Core.EntityFramework.Converters;
using Jazz.Core.EntityFramework.Mappings;
using Jazz.Covenant.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jazz.Core.EntityFramework;

public class CovenantDbContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;

    public CovenantDbContext(DbContextOptions options,
                             ILoggerFactory loggerFactory)
                             : base(options)
    {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    public DbSet<Covenant.Domain.Covenant> Covenants { get; private set; }
    public DbSet<EndosamentMargin> EndosamentMargin { get; private set; }
    public DbSet<MarginEndosamentStatusHistory> MarginEndosamentStatusHistory { get; private set; }
    public DbSet<MarginReserve> MarginReserve { get; private set; }
    public DbSet<MarginReserveStatusHistory> MarginReserveStatusHistory { get; private set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CovenantMap());
        modelBuilder.ApplyConfiguration(new ModalityCovenantMap());
        modelBuilder.ApplyConfiguration(new ModalityMap());
        modelBuilder.ApplyConfiguration(new RegisterNotMarginMap());
        modelBuilder.ApplyConfiguration(new EndosamentMarginMap());
        modelBuilder.ApplyConfiguration(new MarginEndosamentStatusHistoryMap());
        modelBuilder.ApplyConfiguration(new MarginReserveMap());
        modelBuilder.ApplyConfiguration(new MarginReserveStatusHistoryMap());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<PostalCode>().HaveConversion<PostalCodeConverter>();
        configurationBuilder.Properties<CovenantId>().HaveConversion<CovenantIdConverter>();
        configurationBuilder.Properties<NormalizedString>().HaveConversion<NormalizedStringConverter>();
        configurationBuilder.Properties<TaxId>().HaveConversion<TaxIdConverter>();
        configurationBuilder.Properties<Cpf>().HaveConversion<CpfConverter>();
        configurationBuilder.Properties<Cnpj>().HaveConversion<CnpjConverter>();
    }
}
