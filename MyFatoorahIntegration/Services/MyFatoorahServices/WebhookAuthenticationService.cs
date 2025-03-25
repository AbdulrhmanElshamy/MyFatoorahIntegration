using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using System.Text;

namespace MyFatoorahIntegration.Services.MyFatoorahServices
{
    public class WebhookAuthenticationService(
        IConfiguration configuration)
    {
        private readonly string _webhookSecretKey = configuration["MyFatoorah:WebhookSecretKey"];

        public bool VerifyWebhookSignature(HttpRequest request, string requestBody)
        {
            try
            {
                if (!request.Headers.TryGetValue("X-MyFatoorah-Signature", out StringValues signatureHeader))
                {
                    return false;
                }

                var providedSignature = signatureHeader.FirstOrDefault();

                var computedSignature = ComputeHmacSha256Signature(requestBody, _webhookSecretKey);

                bool isSignatureValid = SecureCompare(providedSignature, computedSignature);


                return isSignatureValid;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string ComputeHmacSha256Signature(string payload, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool SecureCompare(string a, string b)
        {
            if (a == null || b == null) return false;

            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }
    }
}
