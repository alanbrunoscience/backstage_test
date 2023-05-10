using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Jazz.Covenant.Domain;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class MarginReserveStatusHistoryMap : IEntityTypeConfiguration<MarginReserveStatusHistory>
    {
        public void Configure(EntityTypeBuilder<MarginReserveStatusHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.MarginReserve)
                .WithMany(x => x.MarginReserveStatusHistory)
                .HasForeignKey(x => x.MarginReserveId);
        }
    }
}
