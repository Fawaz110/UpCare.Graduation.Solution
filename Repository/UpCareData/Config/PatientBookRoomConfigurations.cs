using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.UpCareData.Config
{
    public class PatientBookRoomConfigurations : IEntityTypeConfiguration<PatientBookRoom>
    {
        public void Configure(EntityTypeBuilder<PatientBookRoom> builder)
        {
            builder.HasKey(x => new { x.FK_PatientId, x.FK_DoctorId, x.FK_RoomId });
        }
    }
}
