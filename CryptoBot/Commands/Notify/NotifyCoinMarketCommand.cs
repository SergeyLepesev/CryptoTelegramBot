using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBot.Commands
{
    public class NotifyCoinMarketCommand:ITelegramCommand
    {
        public string Name => "notifycoinmarket";
        public bool IsProtected => true;
        private ITelegramBotClient _client;

        public NotifyCoinMarketCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(Message message)
        {
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new[] // first row
                {
                    new InlineKeyboardCallbackButton("true",$"/{NotifyCoinMarketTrueCommand.NameCommand}"),
                    new InlineKeyboardCallbackButton("false",$"/{NotifyCoinMarketFalseCommand.NameCommand}")
                }
            });
            await _client.SendTextMessageAsync(message.Chat.Id, "Показываем?", replyMarkup: keyboard);
        }
    }
}