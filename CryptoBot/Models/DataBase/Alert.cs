namespace CryptoBot.Models.DataBase
{
    public class Alert
    {
        public string Id { get; set; }
        public long UserId { get; set; }
        public string NameCurrency { get; set; }
        public string PercentNotify { get; set; }
        public double? NotifyCost { get; set; } = null;
        public double? Cost { get; set; } = null;

        public string GetCostReport()
        {
            return NotifyCost == null ? "" : $"{NotifyCost}$";
        }

        public string GetPercentReport()
        {
            return PercentNotify == null ? "" : $"{PercentNotify}%";
        }

        public override string ToString()
        {
            return $"{NameCurrency} {GetCostReport()} {GetPercentReport()}";
        }
    }
}