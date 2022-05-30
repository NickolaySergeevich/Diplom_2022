using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    public class LoginResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
