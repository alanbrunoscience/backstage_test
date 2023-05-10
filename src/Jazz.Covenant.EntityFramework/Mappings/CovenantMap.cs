using Jazz.Covenant.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jazz.Core.EntityFramework.Mappings
{
    public class CovenantMap : IEntityTypeConfiguration<Covenant.Domain.Covenant>
    {
        public void Configure(EntityTypeBuilder<Covenant.Domain.Covenant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(p => p.Endoser)
                .WithMany(p => p.Covenants)
                .HasForeignKey(x => x.EndoserId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            
            

            // Many to Many ModalityCovenant
            builder.HasMany(p => p.AttendedModalities)
                .WithMany(p => p.Covenants)
                .UsingEntity<ModalityCovenant>(
                j => j
                    .HasOne(pt => pt.Modality)
                    .WithMany(t => t.ModalitiesCovenants)
                    .HasForeignKey(pt => pt.ModalityId)
                    .HasPrincipalKey(p => p.Id)
                    .OnDelete(DeleteBehavior.NoAction),
                j => j
                    .HasOne(pt => pt.Covenant)
                    .WithMany(p => p.ModalitiesCovenants)
                    .HasForeignKey(pt => pt.CovenantId)
                    .HasPrincipalKey(p => p.Id)
                    .OnDelete(DeleteBehavior.NoAction),
                j =>
                {
                    j.HasKey(t => t.Id);
                });
        }
    }
}