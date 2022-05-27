using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    public class LoginResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
