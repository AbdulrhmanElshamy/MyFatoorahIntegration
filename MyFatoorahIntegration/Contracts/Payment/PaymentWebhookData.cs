using System.Text.Json.Serialization;

namespace MyFatoorahIntegration.Contracts.Payment
{
    public class PaymentWebhookData
    {
        [JsonPropertyName("InvoiceId")]
        public string InvoiceId { get; set; } = null!;

        [JsonPropertyName("InvoiceStatus")]
        public string InvoiceStatus { get; set; } = null!;

        [JsonPropertyName("InvoiceReference")]
        public string InvoiceReference { get; set; } = null!;

        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; } = null!;

        [JsonPropertyName("CustomerEmail")]
        public string CustomerEmail { get; set; } = null!;

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("Currency")]
        public string Currency { get; set; } = null!;

        [JsonPropertyName("PaymentMethodName")]
        public string PaymentMethodName { get; set; } = null!;

        [JsonPropertyName("PaymentDetails")]
        public PaymentDetails PaymentDetails { get; set; } = null!;
    }

}
