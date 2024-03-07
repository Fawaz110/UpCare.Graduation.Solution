using Core.UpCareEntities.BillEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.UpCareData.Config
{
    public class RadiologyInBillConfigurations : IEntityTypeConfiguration<RadiologyInBill>
    {
        public void Configure(EntityTypeBuilder<RadiologyInBill> builder)
        {
            builder.HasKey(x => new { x.FK_RadiologyId, x.FK_BillId });
        }
    }
}
