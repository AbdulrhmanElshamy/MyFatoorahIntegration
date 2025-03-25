using System.ComponentModel.DataAnnotations;

namespace MyFatoorahIntegration.Contracts.Payment
{
    public class PaymentRequest
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "KWD"; 
        public string NotificationOption { get; set; } = "LNK"; 

        [Required]
        public string CustomerName { get; set; } = null!;

        [Required]
        public string CustomerEmail { get; set; } = null!;

        public string? CustomerPhone { get; set; }
    }

}
