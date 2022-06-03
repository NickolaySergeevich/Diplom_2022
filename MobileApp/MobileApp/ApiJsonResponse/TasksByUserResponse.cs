using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    public class TasksByUserResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("command_name")]
        public string CommandName { get; set; }
        [JsonProperty("is_team_lead")]
        public bool IsTeamLead { get; set; }
    }
}
