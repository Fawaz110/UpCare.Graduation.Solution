using Core.Entities.UpCareEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Contract
{
    public interface ICheckupRepository : IGenericRepository<Checkup>
    {
        Task<Checkup> GetByName(string name);
    }
}
