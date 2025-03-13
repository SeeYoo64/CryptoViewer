using Newtonsoft.Json;

namespace CryptoViewer.Models
{
    public class Market
    {
        [JsonProperty("market")]
        public MarketInfo MarketInfo { get; set; }

        [JsonProperty("last")]
        public decimal LastPrice { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("trade_url")]
        public string TradeUrl { get; set; }

        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        // Adding formatted strong for displaying price with symbol
        [JsonIgnore]
        public string FormattedLastPrice => $"{LastPrice} {Target?.ToUpper()}";
    }

    public class MarketInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }
}