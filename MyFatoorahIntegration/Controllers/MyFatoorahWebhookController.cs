using Microsoft.AspNetCore.Mvc;
using MyFatoorahIntegration.Contracts.Payment;
using MyFatoorahIntegration.Services.MyFatoorahServices;
using System.Text.Json;

namespace MyFatoorahIntegration.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class MyFatoorahWebhookController : ControllerBase
    {
        private readonly IWebhookService _webhookService;
        private readonly WebhookAuthenticationService _authService;

        public MyFatoorahWebhookController(
            IWebhookService webhookService,
            WebhookAuthenticationService authService)
        {
            _webhookService = webhookService;
            _authService = authService;
        }

        [HttpPost("myfatoorah")]
        public async Task<IActionResult> HandleWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            if (!_authService.VerifyWebhookSignature(Request, requestBody))
            {
                return Unauthorized(new { message = "Unauthorized webhook" });
            }

            MyFatoorahWebhookRequest webhookRequest;
            try
            {
                webhookRequest = JsonSerializer.Deserialize<MyFatoorahWebhookRequest>(requestBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid payload" });
            }

            try
            {
                var processed = await _webhookService.ProcessWebhookAsync(webhookRequest);

                return processed
                    ? Ok(new { message = "Webhook processed successfully" })
                    : BadRequest(new { message = "Failed to process webhook" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
