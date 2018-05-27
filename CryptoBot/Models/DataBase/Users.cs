namespace CryptoBot.Models.DataBase
{
    public class Users
    {
        public string Id { get; set; }
        public long TelegramUserId { get; set; }
        public bool Authentication { get; set; }
        public bool NotifyBittrix { get; set; }
        public bool NotifyPoloniex { get; set; }
        public double TimeToCheckBittrix { get; set; } //с каким по времени в прошлом сравнивать
        public double TimeToCheckPoloniex { get; set; }
        public double PoloniexPercentVolume { get; set; }
        public double PoloniexPercentChangeCost { get; set; }
        public double BittrixPercentVolume { get; set; }
        public double BittrixPercentChangeCost { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public long TelegramUserId { get; set; }
        public bool NotifyBittrix { get; set; } = true;
        public bool NotifyPoloniex { get; set; } = true;
        public bool NotifyCoinMarket { get; set; } = true;
        // с каким по времени в прошлом сравнивать
        public double TimeToCheckBittrix { get; set; } = 5;
        public double TimeToCheckPoloniex { get; set; } = 5;
        public double PoloniexPercentVolume { get; set; } = 10;
        public double PoloniexPercentChangeCost { get; set; } = 10;
        public double BittrixPercentVolume { get; set; } = 10;
        public double BittrixPercentChangeCost { get; set; } = 10;
        public double S9Cost { get; set; } = 4500;
        public double L3PlusCost { get; set; } = 1500;
        public double D3Cost { get; set; } = 2700;
        public double B8Cost { get; set; } = 10000;
        public double Nvidia1070 { get; set; } = 4500;
        public double Nvidia1080Ti { get; set; } = 9000;
    }
}