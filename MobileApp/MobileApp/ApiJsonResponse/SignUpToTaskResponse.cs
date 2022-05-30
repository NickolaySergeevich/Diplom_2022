using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    internal class SignUpToTaskResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
