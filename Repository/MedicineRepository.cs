using Core.Repositories.Contract;
using Core.UpCareEntities;
using Repository.UpCareData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(UpCareDbContext context) : base(context)
        {
        }


    }
}
