using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.Models;
using CryptoBot.Models.API;
using CryptoBot.Services;

namespace ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var currencyPairs = await GetCurrencyPairs();

            Console.WriteLine();
            
        }

        private static async Task<double> GetCurrencyPairs()
        {
            var httpClientWrapper = new HttpClientWrapper();
            var miner = new MinerInfo()
            {
                HardwareCost = 1000,
                Name = "s9",
                Id = "1",
                Power = "7000",
                HashRate = "50000"
            };
            var curr1 = await httpClientWrapper.GetBreakEvenIn(miner);

            return curr1;
        }
    }
}