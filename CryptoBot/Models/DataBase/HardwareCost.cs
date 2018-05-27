namespace CryptoBot.Models.DataBase
{
    public class HardwareCost
    {
        public string Id { get; set; }
        public long TelegramUserId { get; set; }
        public double S9Cost { get; set; } = 1000;
        public double L3PlusCost { get; set; } = 1000;
        public double D3Cost { get; set; } = 1000;
        public double B8Cost { get; set; } = 1000;
        public double Nvidia1070 { get; set; } = 1000;
        public double Nvidia1080Ti { get; set; } = 1000;
    }
}