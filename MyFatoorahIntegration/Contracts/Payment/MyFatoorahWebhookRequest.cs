using System.Text.Json.Serialization;

namespace MyFatoorahIntegration.Contracts.Payment
{
    public class MyFatoorahWebhookRequest
    {
        [JsonPropertyName("PaymentData")]
        public PaymentWebhookData PaymentData { get; set; } = null!;
    }

}
