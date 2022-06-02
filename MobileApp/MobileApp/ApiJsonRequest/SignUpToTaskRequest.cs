using System.Collections.Generic;

using Newtonsoft.Json;

namespace MobileApp.ApiJsonRequest
{
    internal class SignUpToTaskRequest
    {
        [JsonProperty("users_id")]
        public List<int> UsersId { get; set; }
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        [JsonProperty("nast_id")]
        public int NastId { get; set; }
        [JsonProperty("command_name")]
        public string CommandName { get; set; }
        [JsonProperty("is_team_lead_id")]
        public int IsTeamLeadId { get; set; }
    }
}
