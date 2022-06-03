using System.Collections.Generic;

using MobileApp.DataTypes;

using Newtonsoft.Json;

namespace MobileApp.ApiJsonResponse
{
    internal class GetTeamsResponse
    {
        [JsonProperty("data")]
        public Dictionary<string, Dictionary<string, List<GetTeamsListType>>> Data { get; set; }
    }
}
