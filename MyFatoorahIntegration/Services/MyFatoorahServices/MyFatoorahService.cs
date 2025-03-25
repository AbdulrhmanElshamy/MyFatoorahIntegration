using MyFatoorahIntegration.Contracts.Payment;
using System.Text.Json;
using System.Text;

namespace MyFatoorahIntegration.Services.MyFatoorahServices
{
    public class MyFatoorahService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public MyFatoorahService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["MyFatoorah:ApiKey"];
            _baseUrl = configuration["MyFatoorah:BaseUrl"];

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<PaymentResponse> CreatePaymentLinkAsync(PaymentRequest request)
        {
            var payload = new
            {
                InvoiceValue = request.Amount,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerMobile = request.CustomerPhone,
                DisplayCurrencyIso = request.Currency,
                CallbackUrl = "https://c385-154-237-5-53.ngrok-free.app/api/webhook/myfatoorah",
                ErrorUrl = "https://yourwebsite.com/payment/error",
                NotificationOption = "LNK"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/v2/SendPayment", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var paymentResponse = JsonSerializer.Deserialize<dynamic>(responseContent);
                    return new PaymentResponse
                    {
                        IsSuccess = true,
                        InvoiceId = paymentResponse.Data.InvoiceId.ToString(),
                        PaymentUrl = paymentResponse.Data.PaymentURL.ToString()
                    };
                }
                else
                {
                    return new PaymentResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent
                    };
                }
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<PaymentResponse> CheckPaymentStatusAsync(string invoiceId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/v2/GetPaymentStatus?invoiceId={invoiceId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var statusResponse = JsonSerializer.Deserialize<dynamic>(responseContent);
                    return new PaymentResponse
                    {
                        IsSuccess = statusResponse.IsSuccess,
                        InvoiceId = invoiceId
                    };
                }
                else
                {
                    return new PaymentResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent
                    };
                }
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
