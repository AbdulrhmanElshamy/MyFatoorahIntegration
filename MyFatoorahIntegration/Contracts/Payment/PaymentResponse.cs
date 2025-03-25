namespace MyFatoorahIntegration.Contracts.Payment
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string InvoiceId { get; set; } =string.Empty;
        public string PaymentUrl { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
