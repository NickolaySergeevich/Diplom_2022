using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    internal class RegistrationResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
