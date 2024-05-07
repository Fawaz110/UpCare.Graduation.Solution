using Core.Services.Contract;
using Core.UpCareEntities;
using Core.UpCareEntities.PrescriptionEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using UpCare.DTOs;
using UpCare.DTOs.PrescriptionDtos;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly UserManager<Doctor> _doctorManager;
        private const string _whSecret = "whsec_6f90552d7e57bd2b37c4a24f189239b8faf5d48d72cbd2f2dfa40db84a8bbbc7";


        public PaymentController(
            IPaymentService paymentService,
            IPrescriptionService prescriptionService,
            UserManager<Doctor> doctorManager)
        {
            _paymentService = paymentService;
            _prescriptionService = prescriptionService;
            _doctorManager = doctorManager;
        }

        [HttpPost("intent")] // POST: /api/payment/intent?prescriptionId={int}&payment={Payment}
        public async Task<ActionResult<Prescription>> CreateOrUpdatePaymentIntent([FromQuery]int prescriptionId, [FromQuery]Payment payment)
        {
            var prescription = await _paymentService.CreateOrUpdatePaymentIntent(prescriptionId, payment);

            if (prescription is null)
                return BadRequest(new ApiResponse(400, "an error occured with your prescription"));

            return Ok(prescription);
        }

        [HttpPost("webhook")] // POST: /api/payment/webhook
        public async Task<IActionResult> CompletePayment()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);

                var paymentIntent = (PaymentIntent) stripeEvent.Data.Object;

                // Handle the event
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        // logic in case of payment succeded
                        await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                        break;

                    case Events.PaymentIntentPaymentFailed:
                        // logic in case of payment failed
                        await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);

                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
