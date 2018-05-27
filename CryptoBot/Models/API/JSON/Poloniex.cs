using Newtonsoft.Json;

namespace CryptoBot.Models.API.JSON
{
    public class Poloniex
    {
        [JsonProperty("baseVolume")]
        public string BaseVolume { get; set; }

        [JsonProperty("high24hr")]
        public string High24Hr { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }

        [JsonProperty("low24hr")]
        public string Low24Hr { get; set; }

        [JsonProperty("percentChange")]
        public string PercentChange { get; set; }

        [JsonProperty("quoteVolume")]
        public string QuoteVolume { get; set; }

        [JsonIgnore]
        public string CurrencyPair { get; set; }
    }
}
