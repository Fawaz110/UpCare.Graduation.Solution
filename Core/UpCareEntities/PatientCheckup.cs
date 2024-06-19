using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities
{
    public class PatientCheckup
    {
        public string FK_PatientId { get; set; }
        public int FK_ChecupId { get; set; }
        public string Result { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
