using Core.UpCareEntities.PrescriptionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.UpCareData.Config
{
    public class CheckupInPrescriptionConfigurations : IEntityTypeConfiguration<CheckupInPrescription>
    {
        public void Configure(EntityTypeBuilder<CheckupInPrescription> builder)
        {
            builder.HasKey(x => new { x.FK_CheckupId, x.FK_PrescriptionId });
        }
    }
}
