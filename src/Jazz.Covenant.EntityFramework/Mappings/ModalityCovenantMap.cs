using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class ModalityCovenantMap : IEntityTypeConfiguration<Covenant.Domain.ModalityCovenant>
    {
        public void Configure(EntityTypeBuilder<Covenant.Domain.ModalityCovenant> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}