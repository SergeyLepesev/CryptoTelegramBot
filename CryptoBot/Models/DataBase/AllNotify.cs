using System;

namespace CryptoBot.Models.DataBase
{
    public class AllNotify
    {
        public long Id { get; set; }
        public DateTime TimeNotify { get; set; }
        public string Report { get; set; }
    }
}