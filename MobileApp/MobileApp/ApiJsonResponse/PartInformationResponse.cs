using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    public class PartInformationResponse : OrgInformationResponse
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
    }
}
