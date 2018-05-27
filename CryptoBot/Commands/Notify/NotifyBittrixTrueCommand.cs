using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CryptoBot.Models.DataBase.User;

namespace CryptoBot.Commands
{
    public class NotifyBittrixTrueCommand:ITelegramCommand
    {
        public string Name => NameCommand;
        public static string NameCommand => "notbitTrue";
        public bool IsProtected => false;
        private ITelegramBotClient _client;
        private IRepository<User> _userRepository;

        public NotifyBittrixTrueCommand(ITelegramBotClient client, IRepository<User> repository)
        {
            _client = client;
            _userRepository = repository;
        }

        public async Task ExecuteAsync(Message message)
        {
            var user = _userRepository.GetItem(z => z.TelegramUserId == message.Chat.Id);
            user.NotifyBittrix = true;
            _userRepository.Update(user,z=>z.TelegramUserId==message.Chat.Id);
            await _client.SendTextMessageAsync(message.Chat.Id, "Success");
        }
    }
}