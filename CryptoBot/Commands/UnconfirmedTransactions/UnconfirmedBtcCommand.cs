using System.Threading.Tasks;
using CryptoBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.Commands.UnconfirmedTransactions
{
    public class UnconfirmedBtcCommand : ITelegramCommand
    {
        private readonly ITelegramBotClient _client;
        private readonly HttpClientWrapper _httpClientWrapper;
        public string Name => "unbtc";
        public bool IsProtected  => true;

        public UnconfirmedBtcCommand(ITelegramBotClient client, HttpClientWrapper httpClientWrapper)
        {
            _client = client;
            _httpClientWrapper = httpClientWrapper;
        }

        public async Task ExecuteAsync(Message message)
        {
            var count = await _httpClientWrapper.GetUnconfirmedBtc();
            if (!string.IsNullOrEmpty(count))
                await _client.SendTextMessageAsync(message.Chat.Id, count);

        }
    }
}