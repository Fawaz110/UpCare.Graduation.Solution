using Core.UpCareEntities.BillEntities;
using Core.UpCareEntities.PrescriptionEntities;

namespace Core.Services.Contract
{
    public enum Payment
    {
        Medicine, 
        Radiology, 
        Checkup, 
        All
    }
    public interface IPaymentService
    {
        Task<Prescription> CreateOrUpdatePaymentIntent(int prescriptionId, Payment payment);
        Task<Bill> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);
    }
}
