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
       //     _httpClient.DefaultRequestHeaders.Add("x-cg-pro-api-key", "CG-FBfRyo6YkUwaPN5wqsJpRE2f");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoViewer/1.0 (test-app by SeeYoo)"); // User Agent for access, otherwise - Forbidden

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

                string url = BaseUrl + "/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=" + count + "&page=1&sparkline=false";
                Console.WriteLine($"Requesting URL: {BaseUrl}{url}");
                await Task.Delay(1000);
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Content: {responseContent}");
                    return new List<Coin>();
                }

                string json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"JSON Response (Top Currencies): {json}"); // Логируем для отладки
                var coins = JsonConvert.DeserializeObject<List<Coin>>(json);
                return coins;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return new List<Coin>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                return new List<Coin>();
            }
        }

        // Get details about specific coin
        public async Task<Coin> GetCoinDetailsAsync(string coinId)
        {
            try
            {
                string url = $"https://api.coingecko.com/api/v3/coins/{coinId}";
        Console.WriteLine($"Requesting URL: {BaseUrl}{url}");
        await Task.Delay(1000);
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"JSON Response (Details): {json}");
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
        Console.WriteLine($"HttpRequestException: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
        return null;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"General Exception: {ex.Message}");
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


        public async Task<List<Market>> GetCoinMarketsAsync(string coinId)
        {
            try
            {
                string url = $"https://api.coingecko.com/api/v3/coins/{coinId}/tickers?exchange_ids=binance%2C%20kraken%2C%20coinbase&page=1&order=volume_desc";
                Console.WriteLine($"Requesting URL: {BaseUrl}{url}");
                await Task.Delay(1000); // Задержка для лимита запросов
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Content: {responseContent}");
                    return new List<Market>();
                }

                string json = await response.Content.ReadAsStringAsync();
                var tickersResponse = JsonConvert.DeserializeObject<dynamic>(json);
                return JsonConvert.DeserializeObject<List<Market>>(tickersResponse.tickers.ToString());
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return new List<Market>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                return new List<Market>();
            }
        }







    }
}
