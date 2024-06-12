using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UpCareEntities
{
    public class UserConnection : BaseEntity
    {
        public string FK_UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
