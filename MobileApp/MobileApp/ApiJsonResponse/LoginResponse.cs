using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    public class LoginResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("users_role_id")]
        public int RoleId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
