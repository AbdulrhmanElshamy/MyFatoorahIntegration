using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFatoorahIntegration.Contracts.Payment;
using MyFatoorahIntegration.Services.MyFatoorahServices;

namespace MyFatoorahIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly MyFatoorahService _myFatoorahService;

        public PaymentController(MyFatoorahService myFatoorahService)
        {
            _myFatoorahService = myFatoorahService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentLink([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _myFatoorahService.CreatePaymentLinkAsync(request);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("status/{invoiceId}")]
        public async Task<IActionResult> CheckPaymentStatus(string invoiceId)
        {
            var result = await _myFatoorahService.CheckPaymentStatusAsync(invoiceId);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
