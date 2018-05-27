using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBot.Commands
{
    public class NotifyBittrixCommand:ITelegramCommand
    {
        private static ITelegramBotClient _client;

        public string Name => "notifybittrix";
        public bool IsProtected => true;

        public NotifyBittrixCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(Message message)
        {
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new[] // first row
                {
                    new InlineKeyboardCallbackButton("false",$"/{NotifyBittrixFalseCommand.NameCommand}"),
                    new InlineKeyboardCallbackButton("true",$"/{NotifyBittrixTrueCommand.NameCommand}")
                }
            });
            await _client.SendTextMessageAsync(message.Chat.Id, "Показываем?", replyMarkup: keyboard);
        }
    }
}