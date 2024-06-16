using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;
using Core.UpCareEntities.PrescriptionEntities;

namespace Core.Services.Contract
{
    public enum PrescriptionPayment
    {
        Medicine, 
        Radiology, 
        Checkup, 
        All
    }
    public interface IPaymentService
    {
        Task<Prescription> CreateOrUpdatePaymentIntent(int prescriptionId, PrescriptionPayment payment);
        Task<Bill> CreateOrUpdatePaymentIntent(Bill bill);
        Task<Bill> CreateOrUpdatePaymentIntent(PatientBookRoom patientBookRoom);
        Task<Bill> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);
    }
}
