using System.Threading.Tasks;
using CryptoBot.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CryptoBot.Models.DataBase.User;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBot.Commands
{
    public class NotifyPoloniexCommand:ITelegramCommand
    {
        private static ITelegramBotClient _client;
       
        public string Name => "notifypoloniex";
        public bool IsProtected => true;

        public NotifyPoloniexCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(Message message)
        {
            
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new[] // first row
                {
                    new InlineKeyboardCallbackButton("false",$"/{NotifyPoloniexFalseCommand.NameCommand}"),
                    new InlineKeyboardCallbackButton("true",$"/{NotifyPoloniexTrueCommand.NameCommand}")
                }
            });
            await _client.SendTextMessageAsync(message.Chat.Id, "Показываем?", replyMarkup: keyboard);
            
        }
    }
}