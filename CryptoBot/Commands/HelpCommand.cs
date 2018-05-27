using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.Commands
{
    public class HelpCommand : ITelegramCommand
    {
        private readonly ITelegramBotClient _client;

        public HelpCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public string Name => "help";
        public bool IsProtected => false;

        public async Task ExecuteAsync(Message message)
        {
            await _client.SendTextMessageAsync(message.Chat.Id, "help");
        }
    }
}