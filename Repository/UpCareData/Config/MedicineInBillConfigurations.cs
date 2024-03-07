using Core.UpCareEntities.BillEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UpCareData.Config
{
    public class MedicineInBillConfigurations : IEntityTypeConfiguration<MedicineInBill>
    {
        public void Configure(EntityTypeBuilder<MedicineInBill> builder)
        {
            builder.HasKey(x => new { x.FK_MedicineId, x.FK_BillId });
        }
    }
}
