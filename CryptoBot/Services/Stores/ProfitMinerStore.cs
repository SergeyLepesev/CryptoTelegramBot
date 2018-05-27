using CryptoBot.Models;

namespace CryptoBot.Services.Stores
{
    public static class ProfitMinerStore
    {
        public static MinerInfo minerS9 = new MinerInfo()
        {
            HashRate = "13500",
            Name = "S9",
            Id = "1",
            Power = "1500"
        };
        public static MinerInfo minerL3 = new MinerInfo()
        {
            HashRate = "504",
            Name = "L3+",
            Id = "4",
            Power = "1100"
        };
        public static MinerInfo minerD3 = new MinerInfo()
        {
            HashRate = "19000",
            Name = "D3",
            Id = "34",
            Power = "800"
        };
        public static MinerInfo minerB8 = new MinerInfo()
        {
            HashRate = "50000",
            Name = "B8",
            Id = "1",
            Power = "6300"
        };
        public static MinerInfo miner1070 = new MinerInfo()
        {
            HashRate = "188",
            Name = "1070",
            Id = "151",
            Power = "1000"
        };
       
        public static MinerInfo miner1080ti = new MinerInfo()
        {
            HashRate = "4500",
            Name = "1080ti",
            Id = "166",
            Power = "1650"
        };
    }
}