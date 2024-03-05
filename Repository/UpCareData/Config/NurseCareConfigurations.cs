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
    public class NurseCareConfigurations : IEntityTypeConfiguration<NurseCare>
    {
        public void Configure(EntityTypeBuilder<NurseCare> builder)
        {
            builder.HasKey(obj => new { obj.FK_NurseId, obj.FK_RoomId, obj.FK_PatientId, obj.DateTime });
        }
    }
}
