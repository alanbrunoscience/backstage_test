using Jazz.Covenant.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class MarginEndosamentStatusHistoryMap : IEntityTypeConfiguration<MarginEndosamentStatusHistory>
    {
        public void Configure(EntityTypeBuilder<MarginEndosamentStatusHistory> builder)
        {

            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.EndosamentMargin)
                    .WithMany(x => x.MarginEndosamentStatusHistorys);

        }
    }
}
