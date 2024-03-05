using Core.Entities.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Data.Config
{
    internal class RadiologyConfigurations : IEntityTypeConfiguration<Radiology>
    {
        public void Configure(EntityTypeBuilder<Radiology> builder)
        {
            builder.Property(rad => rad.Name).IsRequired();

            builder.Property(rad => rad.Name).HasMaxLength(50);

            builder.Property(rad => rad.Price).IsRequired();

            builder.Property(rad => rad.Price).HasColumnType("decimal(18,2)");
        }
    }
}
