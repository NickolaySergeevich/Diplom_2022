using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    internal class UserIdResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
