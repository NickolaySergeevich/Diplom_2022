using Newtonsoft.Json;

namespace MobileApp
{
    internal class LoginResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }
    }
}
