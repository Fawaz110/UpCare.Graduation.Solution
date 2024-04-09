using Core.UpCareEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Contract
{
    public interface IMedicineRepository : IGenericRepository<Medicine>
    {
        Task<Medicine> GetMedicineByNameAsync(string name);
    }
}
