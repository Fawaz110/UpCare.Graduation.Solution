using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities.PrescriptionEntities
{
    public class MedicineInPrescription
    {
        public int FK_MedicineId { get; set; }
        public int FK_PrescriptionId { get; set; }
    }
}
