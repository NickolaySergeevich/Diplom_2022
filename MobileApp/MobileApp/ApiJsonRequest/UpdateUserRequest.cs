using Newtonsoft.Json;

namespace MobileApp.ApiJsonRequest
{
    internal class UpdateUserRequest : RegistrationUserRequest
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}
