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
    internal class PatientCheckupConfigurations : IEntityTypeConfiguration<PatientCheckup>
    {
        public void Configure(EntityTypeBuilder<PatientCheckup> builder)
        {
            builder.HasKey(x => new { x.FK_PatientId, x.FK_ChecupId, x.DateTime });
        }
    }
}
