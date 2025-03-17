using CryptoViewer.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoViewer.Services
{
    public class CoinGeckoService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.coingecko.com/api/v3";
        private readonly string _apiKey;

        public CoinGeckoService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _apiKey = configuration.GetSection("CoinGecko:ApiKey").Value;
            if (string.IsNullOrEmpty(_apiKey))
            {
                Debug.WriteLine("API Key is missing in configuration.");
            }
        }

        // Get X (default 10) currencies with some kind of sort
        public async Task<List<Coin>> GetTopCurrenciesAsync(int count = 10)
        {
            try
            {
                if (count < 1 || count > 250)
                {
                    throw new ArgumentException("per_page must be between 1 and 250", nameof(count)); 
                }

                string url = BaseUrl + "/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=" + count +  $"&page=1&sparkline=false?x_cg_demo_api_key={_apiKey}";
                Debug.WriteLine($"Requesting URL: {BaseUrl}{url}"); // Log the request URL
                await Task.Delay(1000); // Delay for limiting queries
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {responseContent}");
                    return new List<Coin>();
                }

                string json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"JSON Response (Coins): {json}"); // Log the response
                return JsonConvert.DeserializeObject<List<Coin>>(json);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HttpRequestException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return new List<Coin>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Exception: {ex.Message}");
                return new List<Coin>();
            }
        }

        // Get details about specific coin
        public async Task<Coin> GetCoinDetailsAsync(string coinId)
        {
            try
            {
                string url = $"https://api.coingecko.com/api/v3/coins/{coinId}?x_cg_demo_api_key={_apiKey}";
                await Task.Delay(1000); // Delay for limiting queries
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {responseContent}");
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync();
                var coin = JsonConvert.DeserializeObject<Coin>(json);
                if (coin != null && coin.MarketData != null)
                {
                    // Filling properties from MarketData, if they are empty
                    coin.CurrentPrice ??= coin.MarketData.CurrentPrice?["usd"];
                    coin.TotalVolume ??= coin.MarketData.TotalVolume?["usd"];
                    coin.PriceChangePercentage24h ??= coin.MarketData.PriceChangePercentage24h;
                    coin.High24h = coin.MarketData.High24h?["usd"];
                    coin.Low24h = coin.MarketData.Low24h?["usd"];
                }
                    return coin;
                }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HttpRequestException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Exception: {ex.Message}");
                return null;
            }
        }

        // Searching coin 
        public async Task<List<SearchResult>> SearchCoinsAsync(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    return new List<SearchResult>();
                }

                string url = $"https://api.coingecko.com/api/v3/search?query={Uri.EscapeDataString(query)}&x_cg_demo_api_key={_apiKey}";
                Debug.WriteLine(url);
                await Task.Delay(1000); // Delay for limiting queries
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {responseContent}");
                    return new List<SearchResult>();
                }

                string json = await response.Content.ReadAsStringAsync();
                var searchResponse = JsonConvert.DeserializeObject<dynamic>(json);
                return JsonConvert.DeserializeObject<List<SearchResult>>(searchResponse.coins.ToString());
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HttpRequestException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return new List<SearchResult>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Exception: {ex.Message}");
                return new List<SearchResult>();
            }
        }


        public async Task<List<Market>> GetCoinMarketsAsync(string coinId)
        {
            try
            {
                string url = $"https://api.coingecko.com/api/v3/coins/{coinId}" +
                    $"/tickers?exchange_ids=binance%2C%20kraken%2C%20coinbase&page=1&order=volume_desc?x_cg_demo_api_key={_apiKey}";
                Debug.WriteLine($"Requesting URL: {BaseUrl}{url}");
                await Task.Delay(1000); // Delay for limiting queries
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {responseContent}");
                    return new List<Market>();
                }

                string json = await response.Content.ReadAsStringAsync();
                var tickersResponse = JsonConvert.DeserializeObject<dynamic>(json);
                return JsonConvert.DeserializeObject<List<Market>>(tickersResponse.tickers.ToString());
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HttpRequestException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return new List<Market>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Exception: {ex.Message}");
                return new List<Market>();
            }
        }

    }
}
