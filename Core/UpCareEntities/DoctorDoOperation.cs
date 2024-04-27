using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities
{
    public class DoctorDoOperation
    {
        public int FK_OperationId { get; set; }
        public string FK_DoctorId { get; set; }
        public string FK_PatientId { get; set; }
        public string FK_AdminId { get; set; }
        public DateTime Date { get; set; }
    }
}
