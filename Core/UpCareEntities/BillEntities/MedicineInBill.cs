using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities.BillEntities
{
    public class MedicineInBill
    {
        public int FK_MedicineId { get; set; }
        public int FK_BillId { get; set; }
    }
}
