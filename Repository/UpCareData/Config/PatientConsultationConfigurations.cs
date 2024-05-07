using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.UpCareData.Config
{
    public class PatientConsultationConfigurations : IEntityTypeConfiguration<PatientConsultation>
    {
        public void Configure(EntityTypeBuilder<PatientConsultation> builder)
        {
            builder.HasKey(x => new { x.FK_PatientId, x.FK_DoctorId, x.DateTime });
        }
    }
}
