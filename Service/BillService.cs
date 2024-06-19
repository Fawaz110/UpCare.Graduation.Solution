using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;
using Core.UpCareEntities.PrescriptionEntities;

namespace Service
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBillRepository _billRepository;

        public BillService(
            IUnitOfWork unitOfWork, 
            IBillRepository billRepository)
        {
            _unitOfWork = unitOfWork;
            _billRepository = billRepository;
        }
        public async Task<Bill> AddAsync(Bill bill, 
            List<MedicineInPrescription> medicineInPrescriptions,
            List<CheckupInPrescription> checkupInPrescriptions,
            List<RadiologyInPrescription> radiologyInPrescriptions)
        {
            await _unitOfWork.Repository<Bill>().Add(bill);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            if(medicineInPrescriptions.Count() > 0)
            {
                foreach (var item in medicineInPrescriptions)
                {
                    var itemToAdd = new MedicineInBill
                    {
                        FK_MedicineId = item.FK_MedicineId,
                        FK_BillId = bill.Id
                    };

                    await _billRepository.AddMedicineToBillAsync(itemToAdd);
                }
            }

            if(checkupInPrescriptions.Count() > 0)
            {
                foreach (var item in checkupInPrescriptions)
                {
                    var itemToAdd = new CheckupInBill
                    {
                        FK_BillId = bill.Id,
                        FK_CheckupId = item.FK_CheckupId,
                    };

                    await _billRepository.AddCheckupToBillAsync(itemToAdd);
                }
            }

            if(radiologyInPrescriptions.Count() > 0)
            {
                foreach (var item in radiologyInPrescriptions)
                {
                    var itemToAdd = new RadiologyInBill
                    {
                        FK_BillId = bill.Id,
                        FK_RadiologyId = item.FK_RadiologyId,
                    };

                    await _billRepository.AddRadiologyToBillAsync(itemToAdd);
                }
            }

            result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return bill;
        }

        public async Task<Bill> AddAsync(Bill bill)
        {
            await _unitOfWork.Repository<Bill>().Add(bill);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return bill;
        }

        public async Task<List<Checkup>> GetCheckupInBillAsync(int billId)
            => await _billRepository.GetCheckupByBillId(billId);

        public async Task<List<Medicine>> GetMedicineInBillAsync(int billId)
            => await _billRepository.GetMedicineByBillId(billId);

        public async Task<List<Radiology>> GetRadiologiesInBillAsync(int billId)
            => await _billRepository.GetRadiologyByBillId(billId);

        public async Task<Bill> GetWithPaymentIntent(string paymentIntentId)
            => (await _unitOfWork.Repository<Bill>().GetAllAsync()).FirstOrDefault(x => x.PaymentIntentId == paymentIntentId);
    }
}
