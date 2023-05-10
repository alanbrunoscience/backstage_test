using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class ModalityMap : IEntityTypeConfiguration<Covenant.Domain.Modality>
    {
        public void Configure(EntityTypeBuilder<Covenant.Domain.Modality> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}