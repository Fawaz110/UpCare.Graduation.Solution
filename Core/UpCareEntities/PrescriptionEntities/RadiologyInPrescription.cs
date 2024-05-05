using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities.PrescriptionEntities
{
    public class RadiologyInPrescription
    {
        public int FK_RadiologyId { get; set; }
        public int FK_PrescriptionId { get; set; }

    }
}
