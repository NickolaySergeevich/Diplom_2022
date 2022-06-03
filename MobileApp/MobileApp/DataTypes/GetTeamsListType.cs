using Newtonsoft.Json;

namespace MobileApp.DataTypes
{
    internal class GetTeamsListType
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("patronymic")]
        public string Patronymic { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("is_team_lead")]
        public bool IsTeamLead { get; set; }
    }
}
