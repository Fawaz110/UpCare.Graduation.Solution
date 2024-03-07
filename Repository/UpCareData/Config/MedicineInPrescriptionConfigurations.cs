using Core.UpCareEntities.PrescriptionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.UpCareData.Config
{
    public class MedicineInPrescriptionConfigurations : IEntityTypeConfiguration<MedicineInPrescription>
    {
        public void Configure(EntityTypeBuilder<MedicineInPrescription> builder)
        {
            builder.HasKey(x => new { x.FK_MedicineId, x.FK_PrescriptionId });
        }
    }
}
