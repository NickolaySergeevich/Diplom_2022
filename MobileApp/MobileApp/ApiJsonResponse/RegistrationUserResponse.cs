using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    internal class RegistrationUserResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
