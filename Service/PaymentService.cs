using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;
using Core.UpCareEntities.PrescriptionEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stripe;

namespace Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPrescriptionService _prescriptionService;

        public PaymentService(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IPrescriptionService prescriptionService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _prescriptionService = prescriptionService;
        }
        public async Task<Prescription> CreateOrUpdatePaymentIntent(int prescriptionId, Payment payment)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var prescription = await _unitOfWork.Repository<Prescription>().GetByIdAsync(prescriptionId);

            if (prescription is null) return null;

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentintent;

            if(payment == Payment.All)
            {
                if (string.IsNullOrEmpty(prescription.PrescriptionPaymentIntentId)) // Create Prescription PaymentIntent
                {

                    #region Get Medicine Total Price
                    decimal medicineTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.MedicinePaymentIntentId))
                    {
                        var medicineInPrescription = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in medicineInPrescription)
                        {
                            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.FK_MedicineId);

                            medicineTotalPrice += medicine.Price;
                        }
                    }
                    #endregion

                    #region Get Radiology Total Price
                    decimal radiologyTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.RadiologyPaymentIntentId))
                    {
                        var radiologyInPrescription = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in radiologyInPrescription)
                        {
                            var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_RadiologyId);

                            radiologyTotalPrice += radiology.Price;
                        }
                    }
                    #endregion

                    #region Get Checkup Total Price
                    decimal checkupTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.CheckupPaymentIntentId))
                    {
                        var checkupInPrescription = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in checkupInPrescription)
                        {
                            var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_CheckupId);

                            checkupTotalPrice += radiology.Price;
                        }
                    }
                    #endregion

                    var createOptions = new PaymentIntentCreateOptions()
                    {
                        Amount = (long)((medicineTotalPrice + radiologyTotalPrice + checkupTotalPrice) * 100),
                        Currency = "usd",
                        PaymentMethodTypes = new List<string>() { "card" }
                    };

                    paymentintent = await paymentIntentService.CreateAsync(createOptions); // Integrate With Stripe

                    prescription.PrescriptionPaymentIntentId = paymentintent.Id;
                    prescription.PrescriptionClientSecret = paymentintent.ClientSecret;
                }
                else // Update Existing PrescriptionPaymentIntent
                {

                    #region Get Medicine Total Price
                    decimal medicineTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.MedicinePaymentIntentId))
                    {
                        var medicineInPrescription = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in medicineInPrescription)
                        {
                            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.FK_MedicineId);

                            medicineTotalPrice += medicine.Price;
                        }
                    }
                    #endregion

                    #region Get Radiology Total Price
                    decimal radiologyTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.RadiologyPaymentIntentId))
                    {
                        var radiologyInPrescription = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in radiologyInPrescription)
                        {
                            var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_RadiologyId);

                            radiologyTotalPrice += radiology.Price;
                        }
                    }
                    #endregion

                    #region Get Checkup Total Price
                    decimal checkupTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.CheckupPaymentIntentId))
                    {
                        var checkupInPrescription = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in checkupInPrescription)
                        {
                            var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_CheckupId);

                            checkupTotalPrice += radiology.Price;
                        }
                    }
                    #endregion

                    var updateOptions = new PaymentIntentUpdateOptions()
                    {
                        Amount = (long)((medicineTotalPrice + radiologyTotalPrice + checkupTotalPrice) * 100)
                    };

                    await paymentIntentService.UpdateAsync(prescription.PrescriptionPaymentIntentId, updateOptions);
                }

                _unitOfWork.Repository<Prescription>().Update(prescription);

                return prescription;
            }
            else if(payment == Payment.Medicine)
            {
                if (string.IsNullOrEmpty(prescription.PrescriptionPaymentIntentId))
                {
                    if (string.IsNullOrEmpty(prescription.MedicinePaymentIntentId)) // Create Medicine PaymentIntent
                    {

                        #region Get Medicine Total Price
                        decimal medicineTotalPrice = 0m;

                        var medicineInPrescription = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in medicineInPrescription)
                        {
                            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.FK_MedicineId);

                            medicineTotalPrice += medicine.Price;
                        }
                        #endregion

                        var createOptions = new PaymentIntentCreateOptions()
                        {
                            Amount = (long)(medicineTotalPrice * 100),
                            Currency = "usd",
                            PaymentMethodTypes = new List<string>() { "card" }
                        };

                        paymentintent = await paymentIntentService.CreateAsync(createOptions); // Integrate With Stripe

                        prescription.MedicinePaymentIntentId = paymentintent.Id;
                        prescription.MedicineClientSecret = paymentintent.ClientSecret;
                    }
                    else // Update Existing MedicinePaymentIntent
                    {

                        #region Get Medicine Total Price
                        decimal medicineTotalPrice = 0m;

                        var medicineInPrescription = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in medicineInPrescription)
                        {
                            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.FK_MedicineId);

                            medicineTotalPrice += medicine.Price;
                        }
                        #endregion

                        var updateOptions = new PaymentIntentUpdateOptions()
                        {
                            Amount = (long)(medicineTotalPrice * 100)
                        };

                        await paymentIntentService.UpdateAsync(prescription.MedicinePaymentIntentId, updateOptions);
                    }

                    _unitOfWork.Repository<Prescription>().Update(prescription);

                    return prescription;
                }
            }
            else if(payment == Payment.Radiology)
            {
                if (string.IsNullOrEmpty(prescription.PrescriptionPaymentIntentId))
                {
                    if (string.IsNullOrEmpty(prescription.RadiologyPaymentIntentId)) // Create Radiology PaymentIntent
                    {

                        #region Get Radiology Total Price
                        decimal radiologyTotalPrice = 0m;

                        var radiologyInPrescription = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in radiologyInPrescription)
                        {
                            var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_RadiologyId);

                            radiologyTotalPrice += radiology.Price;
                        }
                        #endregion

                        var createOptions = new PaymentIntentCreateOptions()
                        {
                            Amount = (long)(radiologyTotalPrice * 100),
                            Currency = "usd",
                            PaymentMethodTypes = new List<string>() { "card" }
                        };

                        paymentintent = await paymentIntentService.CreateAsync(createOptions); // Integrate With Stripe

                        prescription.RadiologyPaymentIntentId = paymentintent.Id;
                        prescription.RadiologyClientSecret = paymentintent.ClientSecret;
                    }
                    else // Update Existing RadiologyPaymentIntent
                    {
                        #region Get Radiology Total Price
                        decimal radiologyTotalPrice = 0m;

                        var radiologyInPrescription = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in radiologyInPrescription)
                        {
                            var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_RadiologyId);

                            radiologyTotalPrice += radiology.Price;
                        }
                        #endregion

                        var updateOptions = new PaymentIntentUpdateOptions()
                        {
                            Amount = (long)(radiologyTotalPrice * 100)
                        };

                        await paymentIntentService.UpdateAsync(prescription.RadiologyPaymentIntentId, updateOptions);
                    }

                    _unitOfWork.Repository<Prescription>().Update(prescription);

                    return prescription;
                }
            }
            else if(payment == Payment.Checkup)
            {
                if (string.IsNullOrEmpty(prescription.PrescriptionPaymentIntentId))
                {
                    if (string.IsNullOrEmpty(prescription.RadiologyPaymentIntentId)) // Create CHeckup PaymentIntent
                    {

                        #region Get CHeckup Total Price
                        decimal checkupTotalPrice = 0m;

                        var checkupInPrescription = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in checkupInPrescription)
                        {
                            var checkup = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_CheckupId);

                            checkupTotalPrice += checkup.Price;
                        }
                        #endregion

                        var createOptions = new PaymentIntentCreateOptions()
                        {
                            Amount = (long)(checkupTotalPrice * 100),
                            Currency = "usd",
                            PaymentMethodTypes = new List<string>() { "card" }
                        };

                        paymentintent = await paymentIntentService.CreateAsync(createOptions); // Integrate With Stripe

                        prescription.CheckupPaymentIntentId = paymentintent.Id;
                        prescription.CheckupClientSecret = paymentintent.ClientSecret;
                    }
                    else // Update Existing RadiologyPaymentIntent
                    {
                        #region Get Checkup Total Price
                        decimal checkupTotalPrice = 0m;

                        var checkupInPrescription = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in checkupInPrescription)
                        {
                            var checkup = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_CheckupId);

                            checkupTotalPrice += checkup.Price;
                        }
                        #endregion

                        var updateOptions = new PaymentIntentUpdateOptions()
                        {
                            Amount = (long)(checkupTotalPrice * 100)
                        };

                        await paymentIntentService.UpdateAsync(prescription.CheckupPaymentIntentId, updateOptions);
                    }

                    _unitOfWork.Repository<Prescription>().Update(prescription);

                    return prescription;
                }
            }

            _unitOfWork.Repository<Prescription>().Update(prescription);

            return prescription;
        }
    }
}
