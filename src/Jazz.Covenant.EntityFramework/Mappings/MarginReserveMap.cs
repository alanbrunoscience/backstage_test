using Jazz.Covenant.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class MarginReserveMap : IEntityTypeConfiguration<MarginReserve>
    {
        public void Configure(EntityTypeBuilder<MarginReserve> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Covenant)
                .WithMany(x => x.MarginReserves);

            builder.HasOne(x => x.Endoser)
                .WithMany(x => x.MarginReserves);
        }
    }
}