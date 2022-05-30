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
    }
}
