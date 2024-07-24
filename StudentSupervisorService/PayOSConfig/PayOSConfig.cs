using Microsoft.AspNetCore.Http;

namespace StudentSupervisorService.PayOSConfig
{
    public class PayOSConfig
    {
        public readonly string CANCEL_API_URL = "/api/checkout/verify";
        public readonly string RETURN_API_URL = "/api/checkout/verify";
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PayOSConfig(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCancelUrl()
        {
            // Root context path
            var requestURL = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{requestURL.Scheme}://{requestURL.Host}{requestURL.PathBase}";
            return baseUrl + CANCEL_API_URL;
        }

        public string GetReturnUrl()
        {
            // Root context path
            var requestURL = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{requestURL.Scheme}://{requestURL.Host}{requestURL.PathBase}";
            return baseUrl + RETURN_API_URL;
        }
    }
}
