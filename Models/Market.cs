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
    }

    public class MarketInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}