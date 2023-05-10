using Jazz.Covenant.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class EndosamentMarginMap : IEntityTypeConfiguration<EndosamentMargin>
    {
        public void Configure(EntityTypeBuilder<EndosamentMargin> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Covenant)
                .WithMany(x=>x.EndosamentMargins);
            
        }
    }
}