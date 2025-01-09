using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application;

namespace Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ICapPublisher _capPublisher;

        public PaymentController(IPaymentService paymentService, ICapPublisher capPublisher) 
        {
            _paymentService = paymentService;
            _capPublisher = capPublisher;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPaymentRecords(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new { message = "User ID is required." });

            var payments = await _paymentService.GetPaymentRecords(userId);

            if (payments == null)
                return NotFound(new { message = $"Payment records for user Id '{userId}' not found." });

            return Ok(payments);
        }
    }
}
