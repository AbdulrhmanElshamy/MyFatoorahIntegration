using MyFatoorahIntegration.Contracts.Payment;

namespace MyFatoorahIntegration.Services.MyFatoorahServices
{
    public interface IWebhookService
    {
        Task<bool> ProcessWebhookAsync(MyFatoorahWebhookRequest webhookRequest);
    }
}
