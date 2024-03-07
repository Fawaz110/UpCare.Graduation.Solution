using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities.BillEntities
{
    public class RadiologyInBill
    {
        public int FK_RadiologyId { get; set; }
        public int FK_BillId { get; set; }
    }
}
