using Newtonsoft.Json;

namespace MobileApp.ApiJsonRequest
{
    internal class RegistrationRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
    }
}
