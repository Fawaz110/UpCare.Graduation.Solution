using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities.BillEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BillService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Bill> AddAsync(Bill bill)
        {
            await _unitOfWork.Repository<Bill>().Add(bill);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return bill;
        }

        public async Task<Bill> GetWithPaymentIntent(string paymentIntentId)
            => (await _unitOfWork.Repository<Bill>().GetAllAsync()).FirstOrDefault(x => x.PaymentIntentId == paymentIntentId);
    }
}
