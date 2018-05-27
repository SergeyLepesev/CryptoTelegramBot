using System.Threading.Tasks;
using CryptoBot.Models;
using CryptoBot.Models.DataBase;
using CryptoBot.Services.Stores;

namespace CryptoBot.Services
{
    public class ProfitMineService
    {
        private readonly HttpClientWrapper _client;

        public ProfitMineService(HttpClientWrapper client)
        {
            _client = client;
        }

        public void SetCost(User user)
        {
            ProfitMinerStore.miner1070.HardwareCost = user.Nvidia1070;
            ProfitMinerStore.miner1080ti.HardwareCost = user.Nvidia1080Ti;
            ProfitMinerStore.minerS9.HardwareCost = user.S9Cost;
            ProfitMinerStore.minerL3.HardwareCost = user.L3PlusCost;
            ProfitMinerStore.minerD3.HardwareCost = user.D3Cost;
            ProfitMinerStore.minerB8.HardwareCost = user.B8Cost;
        }

        public async Task<string> GetProfit(MinerInfo miner)
        {

            var profit =  await _client.GetBreakEvenIn(miner);
            return $"{profit:F2}";
        }
    }
}