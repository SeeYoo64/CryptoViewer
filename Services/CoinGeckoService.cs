using CryptoViewer.Models;
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
        public CoinGeckoService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoViewer/1.0 (test-app by SeeYoo)"); // User Agent for access, otherwise - Forbidden

        }

        // Get X (default 10) currencies with some kind of sort
        public async Task<List<Coin>> GetTopCurrenciesAsync(int count = 10)
        {

            try
            {
                string url = BaseUrl + "/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=" + count + "&page=1&sparkline=false";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return new List<Coin>();
                }

                string json = await response.Content.ReadAsStringAsync();
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
        public async Task<Coin> GetCoinDetaulsAsync(string coinId)
        {
            try
            {
                string url = $"https://api.coingecko.com/api/v3/coins/{coinId}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Coin>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching coin details: {ex.Message}");
                return null;
            }


        }

        // Searching coin 
        public async Task<List<SearchResult>> SearchCoinsAsync(string query)
        {
            try
            {
                string url = $"/search?query={Uri.EscapeDataString(query)}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var searchResponse = JsonConvert.DeserializeObject<dynamic>(json);
                return JsonConvert.DeserializeObject<List<SearchResult>>(searchResponse.coins.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error searching coins: {ex.Message}");
                return new List<SearchResult>();
            }
        }

    }
}
