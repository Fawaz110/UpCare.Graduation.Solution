using Core.UpCareEntities.BillEntities;

namespace Core.Services.Contract
{
    public interface IBillService
    {
        Task<Bill> AddAsync(Bill bill);
        Task<Bill> GetWithPaymentIntent(string paymentIntentId);
    }
}
