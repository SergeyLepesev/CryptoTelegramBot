namespace CryptoBot.Models
{
    public class Report
    {
        public string Text { get; set; }
        public string CurrencyPair { get; set; }
        public TypeNotice Type { get; set;}
        public override string ToString()
        {
            return $"{Text}";
        }
    }
}