using System;

namespace CryptoBot.Models
{
    public class ReportProfit
    {
        public string Name { get; set; }
        public string Profit { get; set; }
        public override string ToString()
        {
            return Name + new String(' ', 8 - Name.Length) + Profit + " days";
        }
    }
}