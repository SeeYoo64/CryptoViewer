using Newtonsoft.Json;

namespace CryptoViewer.Models
{
    public class Coin
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("market_data")]
        public MarketData MarketData { get; set; }

        [JsonProperty("market_cap_rank")]
        public int? MarketCapRank { get; set; }

        // Adding properties for bindings
        [JsonProperty("current_price")]
        public decimal? CurrentPrice { get; set; }
        [JsonProperty("total_volume")]
        public decimal? TotalVolume { get; set; }
        [JsonProperty("price_change_percentage_24h")]
        public decimal? PriceChangePercentage24h { get; set; }
        public decimal? High24h { get; set; }
        public decimal? Low24h { get; set; }
    }

    public class MarketData
    {
        [JsonProperty("current_price")]
        public Dictionary<string, decimal> CurrentPrice { get; set; }

        [JsonProperty("total_volume")]
        public Dictionary<string, decimal> TotalVolume { get; set; }

        [JsonProperty("price_change_percentage_24h")]
        public decimal? PriceChangePercentage24h { get; set; }

        [JsonProperty("high_24h")]
        public Dictionary<string, decimal> High24h { get; set; }

        [JsonProperty("low_24h")]
        public Dictionary<string, decimal> Low24h { get; set; }

        [JsonProperty("last_updated")]
        public DateTime? LastUpdated { get; set; }
    }
}