using Newtonsoft.Json;

namespace CryptoBot.Models.API.JSON
{
    public class ProfitMine
    {
        public int id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

        [JsonProperty("profit")]
        public string Profit { get; set; }

        [JsonProperty("lagging")]
        public string Lagging { get; set; }
    }

}