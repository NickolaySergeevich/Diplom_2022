using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    internal class UserInformationResponse
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("patronymic")]
        public string Patronymic { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("educational_institution")]
        public string EducationalInstitution { get; set; }
        [JsonProperty("class_number")]
        public int ClassNumber { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
