using Core.Services.Contract;
using Core.UpCareEntities.PrescriptionEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    }
}
