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
        private readonly IBillService _billService;

        public PaymentService(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IPrescriptionService prescriptionService,
            IBillService billService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _prescriptionService = prescriptionService;
            _billService = billService;
        }
        public async Task<Prescription> CreateOrUpdatePaymentIntent(int prescriptionId, PrescriptionPayment payment)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var prescription = await _unitOfWork.Repository<Prescription>().GetByIdAsync(prescriptionId);

            if (prescription is null) return null;

            var bill = new Bill
            {
                DeliveredService = "prescription",
                FK_PayorId = prescription.FK_PatientId,
            };

            var medicineForBill = new List<MedicineInPrescription>();
            var radiologyForBill = new List<RadiologyInPrescription>();
            var checkupForBill = new List<CheckupInPrescription>();

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentintent;

            if(payment == PrescriptionPayment.All)
            {
                if (string.IsNullOrEmpty(prescription.PrescriptionPaymentIntentId)) // Create Prescription PaymentIntent
                {
                    #region Get Medicine Total Price
                    decimal medicineTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.MedicinePaymentIntentId))
                    {
                        medicineForBill = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in medicineForBill)
                        {
                            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.FK_MedicineId);

                            medicine.Quantity -= 1;

                            _unitOfWork.Repository<Medicine>().Update(medicine);

                            medicineTotalPrice += medicine.Price;
                        }
                    }
                    #endregion

                    #region Get Radiology Total Price
                    decimal radiologyTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.RadiologyPaymentIntentId))
                    {
                        radiologyForBill = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in radiologyForBill)
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
                        checkupForBill = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in checkupForBill)
                        {
                            var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(item.FK_CheckupId);

                            checkupTotalPrice += checkup.Price;
                        }
                    }
                    #endregion

                    var createOptions = new PaymentIntentCreateOptions()
                    {
                        Amount = (long)((medicineTotalPrice + radiologyTotalPrice + checkupTotalPrice) * 100),
                        Currency = "usd",
                        PaymentMethodTypes = new List<string>() { "card" }
                    };

                    if (createOptions.Amount > 0)
                    {
                        paymentintent = await paymentIntentService.CreateAsync(createOptions); // Integrate With Stripe
                        prescription.PrescriptionPaymentIntentId = paymentintent.Id;
                        prescription.PrescriptionClientSecret = paymentintent.ClientSecret;
                        bill.PaidMoney = (decimal)(createOptions.Amount / 100);
                        bill.PaymentIntentId = paymentintent.Id;
                        bill.ClientSecret = paymentintent.ClientSecret;
                    }
                }
                else // Update Existing PrescriptionPaymentIntent
                {

                    #region Get Medicine Total Price
                    decimal medicineTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.MedicinePaymentIntentId))
                    {
                        medicineForBill = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in medicineForBill)
                        {
                            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.FK_MedicineId);

                            medicine.Quantity -= 1;

                            _unitOfWork.Repository<Medicine>().Update(medicine);

                            medicineTotalPrice += medicine.Price;
                        }
                    }
                    #endregion

                    #region Get Radiology Total Price
                    decimal radiologyTotalPrice = 0m;

                    if (string.IsNullOrEmpty(prescription.RadiologyPaymentIntentId))
                    {
                        radiologyForBill = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in radiologyForBill)
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
                        checkupForBill = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in checkupForBill)
                        {
                            var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(item.FK_CheckupId);

                            checkupTotalPrice += checkup.Price;
                        }
                    }
                    #endregion

                    var updateOptions = new PaymentIntentUpdateOptions()
                    {
                        Amount = (long)((medicineTotalPrice + radiologyTotalPrice + checkupTotalPrice) * 100)
                    };

                    await paymentIntentService.UpdateAsync(prescription.PrescriptionPaymentIntentId, updateOptions);
                }
            }
            else if(payment == PrescriptionPayment.Medicine)
            {
                if (string.IsNullOrEmpty(prescription.PrescriptionPaymentIntentId))
                {
                    if (string.IsNullOrEmpty(prescription.MedicinePaymentIntentId)) // Create Medicine PaymentIntent
                    {
                        #region Get Medicine Total Price
                        decimal medicineTotalPrice = 0m;

                        medicineForBill = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in medicineForBill)
                        {
                            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.FK_MedicineId);

                            medicine.Quantity -= 1;

                            _unitOfWork.Repository<Medicine>().Update(medicine);

                            medicineTotalPrice += medicine.Price;
                        }
                        #endregion

                        var createOptions = new PaymentIntentCreateOptions()
                        {
                            Amount = (long)(medicineTotalPrice * 100),
                            Currency = "usd",
                            PaymentMethodTypes = new List<string>() { "card" }
                        };
                        if(createOptions.Amount > 0)
                        {
                            paymentintent = await paymentIntentService.CreateAsync(createOptions); // Integrate With Stripe
                            prescription.MedicinePaymentIntentId = paymentintent.Id;
                            prescription.MedicineClientSecret = paymentintent.ClientSecret;
                            bill.PaymentIntentId = paymentintent.Id;
                            bill.ClientSecret = paymentintent.ClientSecret;
                            bill.PaidMoney = (decimal)(createOptions.Amount / 100);
                        }
                    }
                    else // Update Existing MedicinePaymentIntent
                    {

                        #region Get Medicine Total Price
                        decimal medicineTotalPrice = 0m;

                        medicineForBill = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in medicineForBill)
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
                        bill.PaidMoney = (decimal)(updateOptions.Amount / 100);
                    }
                }
            }
            else if(payment == PrescriptionPayment.Radiology)
            {
                if (string.IsNullOrEmpty(prescription.PrescriptionPaymentIntentId))
                {
                    if (string.IsNullOrEmpty(prescription.RadiologyPaymentIntentId)) // Create Radiology PaymentIntent
                    {

                        #region Get Radiology Total Price
                        decimal radiologyTotalPrice = 0m;

                        radiologyForBill = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in radiologyForBill)
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

                        if(createOptions.Amount > 0)
                        {
                            paymentintent = await paymentIntentService.CreateAsync(createOptions); // Integrate With Stripe
                            prescription.RadiologyPaymentIntentId = paymentintent.Id;
                            prescription.RadiologyClientSecret = paymentintent.ClientSecret;
                            bill.PaymentIntentId = paymentintent.Id;
                            bill.ClientSecret = paymentintent.ClientSecret;
                            bill.PaidMoney = (decimal)(createOptions.Amount / 100);
                        }
                    }
                    else // Update Existing RadiologyPaymentIntent
                    {
                        #region Get Radiology Total Price
                        decimal radiologyTotalPrice = 0m;

                        radiologyForBill = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in radiologyForBill)
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
                        bill.PaidMoney = (decimal)(updateOptions.Amount / 100);
                    }

                }
            }
            else if(payment == PrescriptionPayment.Checkup)
            {
                if (string.IsNullOrEmpty(prescription.PrescriptionPaymentIntentId))
                {
                    if (string.IsNullOrEmpty(prescription.CheckupPaymentIntentId)) // Create CHeckup PaymentIntent
                    {

                        #region Get Checkup Total Price
                        decimal checkupTotalPrice = 0m;

                        checkupForBill = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in checkupForBill)
                        {
                            var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(item.FK_CheckupId);

                            checkupTotalPrice += checkup.Price;
                        }
                        #endregion

                        var createOptions = new PaymentIntentCreateOptions()
                        {
                            Amount = (long)(checkupTotalPrice * 100),
                            Currency = "usd",
                            PaymentMethodTypes = new List<string>() { "card" }
                        };

                        if(createOptions.Amount > 0)
                        {
                            paymentintent = await paymentIntentService.CreateAsync(createOptions); // Integrate With Stripe
                            prescription.CheckupPaymentIntentId = paymentintent.Id;
                            prescription.CheckupClientSecret = paymentintent.ClientSecret;
                            bill.PaymentIntentId = paymentintent.Id;
                            bill.ClientSecret = paymentintent.ClientSecret;
                            bill.PaidMoney = (decimal)(createOptions.Amount / 100);
                        }
                    }
                    else // Update Existing RadiologyPaymentIntent
                    {
                        #region Get Checkup Total Price
                        decimal checkupTotalPrice = 0m;

                        var checkupInPrescription = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                        foreach (var item in checkupInPrescription)
                        {
                            var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(item.FK_CheckupId);

                            checkupTotalPrice += checkup.Price;
                        }
                        #endregion

                        var updateOptions = new PaymentIntentUpdateOptions()
                        {
                            Amount = (long)(checkupTotalPrice * 100)
                        };

                        await paymentIntentService.UpdateAsync(prescription.CheckupPaymentIntentId, updateOptions);
                        bill.PaidMoney = (decimal)(updateOptions.Amount / 100);
                    }
                }
            }


            await _billService.AddAsync(bill, medicineForBill, checkupForBill, radiologyForBill);

            _unitOfWork.Repository<Prescription>().Update(prescription);

            var result = await _unitOfWork.CompleteAsync();

            return (result <= 0)? null : prescription;
        }

        public async Task<Bill> CreateOrUpdatePaymentIntent(Bill bill)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(bill.PaymentIntentId)) // Create PaymentIntent
            {
                PaymentIntentCreateOptions options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(bill.PaidMoney * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);
                bill.PaymentIntentId = paymentIntent.Id;
                bill.ClientSecret = paymentIntent.ClientSecret;
            } // Create PaymentIntent
            else // Update PaymentIntent
            {
                PaymentIntentUpdateOptions options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(bill.PaidMoney * 100)
                };

                await paymentIntentService.UpdateAsync(bill.PaymentIntentId, options);
            } // Update PaymentIntent

            await _unitOfWork.Repository<Bill>().Add(bill);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return bill;
        }

        public async Task<Bill> CreateOrUpdatePaymentIntent(PatientBookRoom patientBookRoom)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(patientBookRoom.FK_RoomId);

            if (room is null)
                return null;

            var bill = new Bill()
            {
                DateTime = DateTime.Now,
                DeliveredService = "room reservation",
                FK_PayorId = patientBookRoom.FK_PatientId,
            };

            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(bill.PaymentIntentId)) // Create PaymentIntent
            {

                TimeSpan diff = bill.DateTime - patientBookRoom.StartDate;

                var period = Math.Abs(diff.Days);

                PaymentIntentCreateOptions options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(period * room.PricePerNight * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                if(options.Amount > 0)
                {
                    paymentIntent = await paymentIntentService.CreateAsync(options);
                    bill.PaymentIntentId = paymentIntent.Id;
                    bill.ClientSecret = paymentIntent.ClientSecret;
                }
                else
                {
                    return null;
                }

            } // Create PaymentIntent
            else // Update PaymentIntent
            {
                PaymentIntentUpdateOptions options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(bill.PaidMoney * 100)
                };

                await paymentIntentService.UpdateAsync(bill.PaymentIntentId, options);
            } // Update PaymentIntent

            await _unitOfWork.Repository<Bill>().Add(bill);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return bill;
        }

        public async Task<Bill> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
            => await _billService.GetWithPaymentIntent(paymentIntentId);
    }
}
