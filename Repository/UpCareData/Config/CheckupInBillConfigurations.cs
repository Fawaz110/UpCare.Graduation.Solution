using Core.UpCareEntities.BillEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.UpCareData.Config
{
    public class CheckupInBillConfigurations : IEntityTypeConfiguration<CheckupInBill>
    {
        public void Configure(EntityTypeBuilder<CheckupInBill> builder)
        {
            builder.HasKey(x => new { x.FK_CheckupId, x.FK_BillId });
        }
    }
}
