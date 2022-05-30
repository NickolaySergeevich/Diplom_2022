using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    public class TasksResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("teams_count")]
        public int TeamsCount { get; set; }
        [JsonProperty("team_member_max")]
        public int TeamMemberMax { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("essay")]
        public bool IsEssay { get; set; }
        [JsonProperty("test")]
        public bool IsTest { get; set; }
    }
}
