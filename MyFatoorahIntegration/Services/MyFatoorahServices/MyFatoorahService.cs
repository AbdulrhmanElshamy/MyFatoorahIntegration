using MyFatoorahIntegration.Contracts.Payment;
using System.Text.Json;
using System.Text;
using System.Net.Http;

namespace MyFatoorahIntegration.Services.MyFatoorahServices
{
    public class MyFatoorahService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _callBack;
        private readonly string _errorUrl;

        public MyFatoorahService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["MyFatoorah:ApiKey"];
            _baseUrl = configuration["MyFatoorah:BaseUrl"];
            _callBack = configuration["MyFatoorah:CallBack"];
            _errorUrl = configuration["MyFatoorah:ErrorUrl"];

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
                CallbackUrl = _callBack,
                ErrorUrl = _errorUrl,
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
                    // Use the correct model structure for deserialization
                    var paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return paymentResponse;
                }
                else
                {
                    // Try to deserialize error response
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<PaymentResponse>(responseContent,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        return errorResponse;
                    }
                    catch
                    {
                        // If error response cannot be deserialized, return generic error
                        return new PaymentResponse
                        {
                            IsSuccess = false,
                            Message = $"Error: {response.StatusCode}. {responseContent}"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = $"Exception occurred: {ex.Message}"
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
                        Data = new PaymentData
                        {

                            InvoiceId = statusResponse.Data.InvoiceId.ToString(),
                            InvoiceURL = statusResponse.Data.InvoiceURL.ToString(),
                            CustomerReference = statusResponse.Data.CustomerReference.ToString(),
                            UserDefinedField = statusResponse.Data.UserDefinedField.ToString(),
                        },
                    };
                }
                else
                {
                    return new PaymentResponse
                    {
                        IsSuccess = false,
                    };
                }
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    IsSuccess = false,
       
                };
            }
        }
    }
}
