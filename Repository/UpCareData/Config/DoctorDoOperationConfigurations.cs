using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UpCareData.Config
{
    public class DoctorDoOperationConfigurations : IEntityTypeConfiguration<DoctorDoOperation>
    {
        public void Configure(EntityTypeBuilder<DoctorDoOperation> builder)
        {
            builder.HasKey(x => new { x.FK_PatientId, x.FK_DoctorId, x.FK_OperationId, x.Date });
        }
    }
}
