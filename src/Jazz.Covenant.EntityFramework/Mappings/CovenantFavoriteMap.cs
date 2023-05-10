using Jazz.Covenant.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class CovenantFavoriteMap : IEntityTypeConfiguration<CovenantFavorite>
    {
        public void Configure(EntityTypeBuilder<CovenantFavorite> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Covenant)
                   .WithMany(x => x.CovenantFavorites);
        }
    }
}
