using Jazz.Covenant.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class EndoserMap : IEntityTypeConfiguration<Endoser>
    {
        public void Configure(EntityTypeBuilder<Endoser> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}