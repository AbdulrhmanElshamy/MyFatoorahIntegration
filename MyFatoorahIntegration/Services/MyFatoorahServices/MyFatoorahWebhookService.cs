using MyFatoorahIntegration.Contracts.Payment;
using System.Text.Json;

namespace MyFatoorahIntegration.Services.MyFatoorahServices
{
    public class MyFatoorahWebhookService : IWebhookService
    {

        public async Task<bool> ProcessWebhookAsync(MyFatoorahWebhookRequest webhookRequest)
        {
            if (webhookRequest?.PaymentData == null)
            {
                return false;
            }

            try
            {
                switch (webhookRequest.PaymentData.InvoiceStatus.ToLower())
                {
                    case "paid":
                        await HandleSuccessfulPaymentAsync(webhookRequest.PaymentData);
                        break;
                    case "failed":
                        await HandleFailedPaymentAsync(webhookRequest.PaymentData);
                        break;
                    default:
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task HandleSuccessfulPaymentAsync(PaymentWebhookData paymentData)
        {
            // TODO: Mark Order As Paid

        }

        private async Task HandleFailedPaymentAsync(PaymentWebhookData paymentData)
        {

            // TODO: Mark Order As Failed

        }
    }
}
