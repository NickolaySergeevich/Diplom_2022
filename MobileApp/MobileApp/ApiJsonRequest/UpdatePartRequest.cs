using Newtonsoft.Json;

namespace MobileApp.ApiJsonRequest
{
    internal class UpdatePartRequest : UpdateOrgRequest
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
    }
}
