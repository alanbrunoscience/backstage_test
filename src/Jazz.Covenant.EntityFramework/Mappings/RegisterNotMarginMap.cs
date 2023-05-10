using Jazz.Covenant.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class RegisterNotMarginMap : IEntityTypeConfiguration<RegisterNotMargin>
    {
        public void Configure(EntityTypeBuilder<RegisterNotMargin> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Covenant)
                   .WithMany(x => x.RegisterMargins);
        }
    }
}
