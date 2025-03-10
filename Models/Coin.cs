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

        [JsonProperty("current_price")]
        public decimal CurrentPrice { get; set; }

        [JsonProperty("market_cap_rank")]
        public int? MarketCapRank { get; set; }

        [JsonProperty("total_volume")]
        public decimal TotalVolume { get; set; }

        [JsonProperty("price_change_percentage_24h")]
        public decimal? PriceChangePercentage24h { get; set; }
    }
}