using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoBot.Models;
using CryptoBot.Models.API;
using CryptoBot.Models.API.JSON;
using Newtonsoft.Json;

namespace CryptoBot.Services
{
    public class HttpClientWrapper
    {
        private readonly HttpClient _client;

        public HttpClientWrapper()
        {
            _client = new HttpClient();
        }

        private async Task<T> GetAsync<T>(string url)
        {
            var responce = await _client.GetAsync(url);
            var content = await responce.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<IEnumerable<CurrencyPair>> GetBittrix()
        {
            var currency = await GetAsync<Bittrix>("https://bittrex.com/api/v1.1/public/getmarketsummaries");

            return currency.result
                .Where(z => z.MarketName.Contains("BTC-"))
                .ToArray().Select(CurrencyPairMapper.Map);
        }

        public async Task<IEnumerable<CurrencyPair>> GetPoloniex()
        {
            var url = "https://poloniex.com/public?command=returnTicker";
            var currencyPairs = await GetAsync<Dictionary<string, Poloniex>>(url);

            return currencyPairs
                .Where(z => z.Key.Contains("USDT_"))
                .Select(x =>
                {
                    x.Value.CurrencyPair = x.Key;
                    return x.Value;
                })
                .Select(CurrencyPairMapper.Map);
        }

        public async Task<IEnumerable<CurrencyPair>> GetCoinMarket()
        {
            var url = "https://api.coinmarketcap.com/v1/ticker/?limit=100";
            var currencyPairs = await GetAsync<IEnumerable<CoinMarket>>(url);

            return currencyPairs.Select(CurrencyPairMapper.Map);
        }

        public async Task<string> GetUnconfirmedBtc()
        {
            var response = await _client.GetAsync("https://blockchain.info/unconfirmed-transactions");
            var stream = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var s = reader.ReadLine();
                    if (s.Contains("Unconfirmed"))
                        return new string(s.Where(char.IsDigit).ToArray());
                }
            }
            return "";
        }

        public async Task<double> GetBreakEvenIn(MinerInfo minerInfo)
        {
            var profitMine = await GetAsync<ProfitMine>(minerInfo.ToString());
            var profit = Double.Parse(profitMine.Profit.Replace("$",""), CultureInfo.InvariantCulture);

            return minerInfo.HardwareCost / profit;

        }
    }
}