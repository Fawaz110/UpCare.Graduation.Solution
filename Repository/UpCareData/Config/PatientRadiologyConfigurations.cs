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
    internal class PatientRadiologyConfigurations : IEntityTypeConfiguration<PatientRadiology>
    {
        public void Configure(EntityTypeBuilder<PatientRadiology> builder)
        {
            builder.HasKey(x => new { x.FK_PatientId, x.FK_RadiologyId, x.DateTime });
        }
    }
}
