﻿using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    internal class TasksResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("teams_count")]
        public int TeamsCount { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("essay")]
        public bool IsEssay { get; set; }
        [JsonProperty("test")]
        public bool IsTest { get; set; }
    }
}
