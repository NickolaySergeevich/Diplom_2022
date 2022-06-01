using System;

using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    internal class ChatResponse
    {
        [JsonProperty("chats.date")]
        public DateTime Date { get; set; }
        [JsonProperty("chats.text")]
        public string Text { get; set; }
        [JsonProperty("mine")]
        public bool Mine { get; set; }
    }
}
