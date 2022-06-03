using Newtonsoft.Json;

namespace MobileApp.ApiJsonRequest
{
    internal class UpdateOrgRequest
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("patronymic")]
        public string Patronymic { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
