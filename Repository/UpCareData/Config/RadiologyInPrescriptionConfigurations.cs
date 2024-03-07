using Core.UpCareEntities.PrescriptionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UpCareData.Config
{
    public class RadiologyInPrescriptionConfigurations : IEntityTypeConfiguration<RadiologyInPrescription>
    {
        public void Configure(EntityTypeBuilder<RadiologyInPrescription> builder)
        {
            builder.HasKey(x => new { x.FK_PrescriptionId, x.FK_RadiologyId });
        }
    }
}
