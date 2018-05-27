using System.Threading.Tasks;
using CryptoBot.Services.Stores;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CryptoBot.Commands
{
    public class InformationCommand:ITelegramCommand
    {
        public static string NameCommand = "Information";
        public string Name => NameCommand;
        public bool IsProtected => true;

        private ITelegramBotClient _client;

        public InformationCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(Message message)
        {
            var currencyPairs = CurrencyInfoStore.GetLastCoinMarket();
            if (currencyPairs == null)
            {
                await _client.SendTextMessageAsync(message.Chat.Id, "Some problemes");
                return;
            }
            foreach (var pair in currencyPairs)
            {
                if (string.Equals(pair.FirstCurrency,message.Text.ToUpper()))
                {
                    await _client.SendTextMessageAsync(message.Chat.Id, pair.GetInfo(),ParseMode.Markdown);
                    return;
                }
            }
            await _client.SendTextMessageAsync(message.Chat.Id, "Currency not find");
        }
    }
}