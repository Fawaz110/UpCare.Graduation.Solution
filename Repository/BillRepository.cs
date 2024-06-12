using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;

namespace Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly UpCareDbContext _context;

        public BillRepository(
            UpCareDbContext context)
        {
            _context = context;
        }

        public async Task AddCheckupToBillAsync(CheckupInBill checkupInBill)
           => await _context.Set<CheckupInBill>().AddAsync(checkupInBill);

        public async Task AddMedicineToBillAsync(MedicineInBill medicineInBill)
            => await _context.Set<MedicineInBill>().AddAsync(medicineInBill);

        public async Task AddRadiologyToBillAsync(RadiologyInBill radiologyInBill)
            => await _context.Set<RadiologyInBill>().AddAsync(radiologyInBill);

        public async Task<List<Checkup>> GetCheckupByBillId(int billId)
        {
            var listOfRelation = await _context.Set<CheckupInBill>().Where(x => x.FK_BillId == billId).ToListAsync();

            var checkupList = new List<Checkup>();

            foreach (var relation in listOfRelation)
            {
                var checkup = await _context.Set<Checkup>().FirstOrDefaultAsync(x => x.Id == relation.FK_CheckupId);

                checkupList.Add(checkup);
            }

            return checkupList;
        }

        public async Task<List<Medicine>> GetMedicineByBillId(int billId)
        {
            var listOfRelation = await _context.Set<MedicineInBill>().Where(x => x.FK_BillId == billId).ToListAsync();

            var medicineList = new List<Medicine>();

            foreach (var relation in listOfRelation)
            {
                var medicine = await _context.Set<Medicine>().FirstOrDefaultAsync(x => x.Id == relation.FK_MedicineId);

                medicineList.Add(medicine);
            }

            return medicineList;
        }

        public async Task<List<Radiology>> GetRadiologyByBillId(int billId)
        {
            var listOfRelation = await _context.Set<RadiologyInBill>().Where(x => x.FK_BillId == billId).ToListAsync();

            var radiologyList = new List<Radiology>();

            foreach (var relation in listOfRelation)
            {
                var radiology = await _context.Set<Radiology>().FirstOrDefaultAsync(x => x.Id == relation.FK_RadiologyId);

                radiologyList.Add(radiology);
            }

            return radiologyList;
        }
    }
}
