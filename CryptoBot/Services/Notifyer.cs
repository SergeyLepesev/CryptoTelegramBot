using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBot.Models;
using Telegram.Bot;

namespace CryptoBot.Services
{
    public class Notifyer
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public Notifyer(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task Notify(Notification notification)
        {
            await _telegramBotClient
                .SendTextMessageAsync(notification.ChatId, notification.ToString());
        }

        public async Task Notify(IEnumerable<Notification> notifications)
        {
            foreach (var notification in notifications)
                await _telegramBotClient
                    .SendTextMessageAsync(notification.ChatId, notification.ToString());
        }
    }
}