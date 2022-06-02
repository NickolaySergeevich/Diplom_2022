using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    internal class WorkWithTaskResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
