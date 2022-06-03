using Newtonsoft.Json;

namespace MobileApp.ApiJsonRequest
{
    internal class RemoveFromTaskRequest
    {
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        [JsonProperty("command_name")]
        public string CommandName { get; set; }
    }
}
