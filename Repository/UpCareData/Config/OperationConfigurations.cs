using Core.Entities.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.UpCareData.config
{
    public class OperationConfigurations : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.Property(m => m.Price).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}
