using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoBot.Models
{
    public class Notification
    {
        public Notification(long chatId, IEnumerable<Report> reports)
        {
            ChatId = chatId;
            Reports = reports;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Reports.Select(x => x.ToString()));
        }

        public long ChatId { get; }
        public IEnumerable<Report> Reports { get; }
    }
}