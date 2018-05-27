namespace CryptoBot.Models.API
{
    public class CurrencyPair
    {
        public string CurrencyPairName { get; set; }
        public string Name { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }
        public double Last { get; set; }
        public double BaseVolume { get; set; }
        public double PercentChange { get; set; }
        public string FirstCurrency { get; set; }
        public string SecondCurrency { get; set; }

        public string GetInfo()
        {
            return $"{Name}({CurrencyPairName})\n"+
                $"*{Last:F2}$*\n"
                +$"_{PercentChange}%_";
        }
    }
}