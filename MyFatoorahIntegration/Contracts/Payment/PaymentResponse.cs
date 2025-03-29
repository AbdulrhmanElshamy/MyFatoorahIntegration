namespace MyFatoorahIntegration.Contracts.Payment
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
        public PaymentData Data { get; set; }
    }

    public class ValidationError
    {
        public string Name { get; set; }
        public string Error { get; set; }
    }

    public class PaymentData
    {
        public int InvoiceId { get; set; }
        public string InvoiceURL { get; set; }
        public string CustomerReference { get; set; }
        public string UserDefinedField { get; set; }
    }
}
