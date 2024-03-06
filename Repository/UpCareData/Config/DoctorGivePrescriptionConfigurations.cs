using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.UpCareData.Config
{
    public class DoctorGivePrescriptionConfigurations : IEntityTypeConfiguration<DoctorGivePrescription>
    {
        public void Configure(EntityTypeBuilder<DoctorGivePrescription> builder)
        {
            builder.HasKey(x => new { x.FK_PatientId, x.FK_PrescriptionId, x.FK_DoctorId });
        }
    }
}
