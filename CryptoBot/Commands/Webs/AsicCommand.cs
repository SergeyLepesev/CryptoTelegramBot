using System;
using System.Threading.Tasks;
using CryptoBot.Data;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.Commands.Webs
{
    public class AsicCommand:ITelegramCommand
    {
        public string Name => "asic";
        public bool IsProtected => true;

        private ITelegramBotClient _client;

        public AsicCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(Message message)
        {
            var url = Environment.GetEnvironmentVariable("HEROKU_URL");
            await _client.SendTextMessageAsync(message.Chat.Id, $"{url}/asic/settings?={message.Chat.Id}");
        }
    }
}