using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities
{
    public class PatientBookRoom
    {
        public int FK_RoomId { get; set; }
        public string FK_PatientId { get; set; }
        public string FK_DoctorId { get; set; }
        public DateTime StartDate { get; set;} = DateTime.UtcNow;
        public DateTime EndDate { get; set;}

    }
}
