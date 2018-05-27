
namespace CryptoBot.Models.DataBase
{
    public class Asic
    {
        public string Id { get; set; }
        public long TelegramUserId { get; set; }
        public double Sha256_HR { get; set; }
        public double Sha256_P { get; set; }
        public double Scrypt_HR { get; set; }
        public double Scrypt_P { get; set; }
        public double X11_HR { get; set; }
        public double X11_P { get; set; }
        public double Quark_HR { get; set; }
        public double Quark_P { get; set; }
        public double Qubit_HR { get; set; }
        public double Qubit_P { get; set; }
        public double Cost { get; set; }
    }
}