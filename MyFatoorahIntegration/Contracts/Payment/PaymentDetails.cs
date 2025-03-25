using System.Text.Json.Serialization;

namespace MyFatoorahIntegration.Contracts.Payment
{
    public class PaymentDetails
    {
        [JsonPropertyName("TransactionId")]
        public string TransactionId { get; set; } = null!;

        [JsonPropertyName("PaymentDate")]
        public DateTime PaymentDate { get; set; }
    }

}
