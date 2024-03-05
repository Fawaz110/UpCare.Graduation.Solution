using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.UpCareData.Config
{
    public class MedicineConfigurations : IEntityTypeConfiguration<Medicine>
    {
        public void Configure(EntityTypeBuilder<Medicine> builder)
        {
            builder.Property(m => m.Name).IsRequired();

            builder.Property(m => m.Price).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(m => m.SideEffects).IsRequired();

            builder.Property(m => m.Indecations).IsRequired();

            builder.Property(m => m.Indecations).IsRequired();

            builder.Property(m => m.ProductionDate).IsRequired();

            builder.Property(m => m.ExpiryDate).IsRequired();

            builder.Property(m => m.Category).IsRequired();
        }
    }
}
