using Core.Services.Contract;
using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;
using Core.UpCareEntities.PrescriptionEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using UpCare.DTOs;
using UpCare.DTOs.BillDtos;
using UpCare.DTOs.PrescriptionDtos;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly UserManager<Patient> _patientManager;
        private const string _whSecret = "whsec_6f90552d7e57bd2b37c4a24f189239b8faf5d48d72cbd2f2dfa40db84a8bbbc7";


        public PaymentController(
            IPaymentService paymentService,
            IPrescriptionService prescriptionService,
            UserManager<Doctor> doctorManager,
            UserManager<Patient> patientManager)
        {
            _paymentService = paymentService;
            _prescriptionService = prescriptionService;
            _doctorManager = doctorManager;
            _patientManager = patientManager;
        }

        [HttpPost("intent")] // POST: /api/payment/intent?prescriptionId={int}&payment={Payment}
        public async Task<ActionResult<Prescription>> CreateOrUpdatePrescriptionPaymentIntent([FromQuery]int prescriptionId, [FromQuery] PrescriptionPayment payment)
        {
            var prescription = await _paymentService.CreateOrUpdatePaymentIntent(prescriptionId, payment);

            if (prescription is null)
                return BadRequest(new ApiResponse(400, "an error occured with your prescription"));

            return Ok(prescription);
        }

        [HttpPost("intent/reservation")] // POST: /api/payment/intent/reservation
        public async Task<ActionResult<SucceededToAdd>> CreateOrUpdateReservationPaymentIntent([FromBody] Bill model)
        {
            var patient = await _patientManager.FindByIdAsync(model.FK_PayorId);

            if (patient is null)
                return NotFound(new ApiResponse(404, "no patient matches found"));

            var bill = await _paymentService.CreateOrUpdatePaymentIntent(model);

            if (bill is null) 
                return BadRequest(new ApiResponse(400, "an error occured during adding bill"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = new ReservationBillDto
                {
                    DateTime = bill.DateTime,
                    DeliveredService = bill.DeliveredService,
                    ClientSecret = bill.ClientSecret,
                    PaymentIntentId = bill.PaymentIntentId,
                    Payor = patient,
                    Id = bill.Id
                }
            });
        }

        [HttpPost("intent/booking")] // POST: /api/payment/intent/booking
        public async Task<ActionResult<SucceededToAdd>> CreateOrUpdateBookingPaymentIntent([FromBody] PatientBookRoom model)
        {
            var patient = await _patientManager.FindByIdAsync(model.FK_PatientId);

            if (patient is null)
                return NotFound(new ApiResponse(404, "no patient matches found"));

            var bill = await _paymentService.CreateOrUpdatePaymentIntent(model);

            if (bill is null) 
                return BadRequest(new ApiResponse(400, "no payment done"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = bill
            });
        }

        #region Commented Webhook
        //[HttpPost("webhook")] // POST: /api/payment/webhook
        //public async Task<IActionResult> CompletePayment()
        //{
        //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //    try
        //    {
        //        var stripeEvent = EventUtility.ConstructEvent(json,
        //            Request.Headers["Stripe-Signature"], _whSecret);

        //        var paymentIntent = (PaymentIntent) stripeEvent.Data.Object;

        //        // Handle the event
        //        switch (stripeEvent.Type)
        //        {
        //            case Events.PaymentIntentSucceeded:
        //                // logic in case of payment succeded
        //                await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
        //                break;

        //            case Events.PaymentIntentPaymentFailed:
        //                // logic in case of payment failed
        //                await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
        //                break;
        //        }

        //        return Ok();
        //    }
        //    catch (StripeException e)
        //    {
        //        return BadRequest();
        //    }
        //} 
        #endregion
    }
}
