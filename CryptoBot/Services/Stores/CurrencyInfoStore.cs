using System;
using System.Collections.Generic;
using System.Linq;
using CryptoBot.Models.API;

namespace CryptoBot.Services.Stores
{
    public static class CurrencyInfoStore
    {
        public static List<IEnumerable<CurrencyPair>> Poloniexes { get; } = new List<IEnumerable<CurrencyPair>>();

        public static List<IEnumerable<CurrencyPair>> Bittrixes { get; } = new List<IEnumerable<CurrencyPair>>();

        public static List<IEnumerable<CurrencyPair>> CoinMarkets { get; } = new List<IEnumerable<CurrencyPair>>();


        public static void AddBittrix(IEnumerable<CurrencyPair> bittrix)
        {
            if (Bittrixes.Count >= 200)
                Bittrixes.RemoveAt(0);

            Bittrixes.Add(bittrix);
        }

        public static void AddPoloniex(IEnumerable<CurrencyPair> poloniex)
        {
            if (Poloniexes.Count >= 10)
                Poloniexes.RemoveAt(0);

            Poloniexes.Add(poloniex);
        }

        public static void AddCoinMarket(IEnumerable<CurrencyPair> coinMarket)
        {
            if (CoinMarkets.Count >= 10)
                CoinMarkets.RemoveAt(0);
            CoinMarkets.Add(coinMarket);
        }
        public static string GetLastPoloneix()
        {
            var lengthMaxName = Poloniexes.Last().Select(x => x.SecondCurrency.Length).Max();
            var lengthMaxLast = Poloniexes.Last().Select(x => $"{x.Last:F2}".Length).Max();
            var lengthMaxPercent = Poloniexes.Last().Select(x => $"{x.PercentChange:F2}".Length).Max();

            string FormatItem(CurrencyPair poloniex) =>
                "`"+$"{poloniex.SecondCurrency}" + new string(' ', 3 + lengthMaxName - poloniex.SecondCurrency.Length) +
                $"{poloniex.Last:F2}$" + new string(' ', 3 + lengthMaxLast - $"{poloniex.Last:F2}".Length) +
                $"{(100*poloniex.PercentChange):F2}%" + new string(' ', 3 + lengthMaxPercent - $"{poloniex.PercentChange:F2}".Length)+"`";
            //+ $"{poloniex.BaseVolume:F2}";


            return string.Join(Environment.NewLine+ Environment.NewLine, Poloniexes.Last().OrderByDescending(z=>z.BaseVolume).Select(x => FormatItem(x)));
        }

        public static IEnumerable<CurrencyPair> GetLastCoinMarket()
        {
            return CoinMarkets.LastOrDefault();
        }
    }
}