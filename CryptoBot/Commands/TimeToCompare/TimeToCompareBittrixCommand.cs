using System.Threading.Tasks;
using CryptoBot.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CryptoBot.Models.DataBase.User;

namespace CryptoBot.Commands.TimeToCompare
{
    public class TimeToCompareBittrixCommand:ITelegramCommand
    {
        public string Name => "bittrixtimecompare";
        public bool IsProtected => true;

        private readonly ITelegramBotClient _client;
        private readonly IRepository<User> _userRepository;

        public TimeToCompareBittrixCommand(ITelegramBotClient client, IRepository<User> repository)
        {
            _client = client;
            _userRepository = repository;
        }

        public async Task ExecuteAsync(Message message)
        {
            var timeStr = message.Text.TrimEnd().Split(' ');
            if (timeStr.Length > 1)
                if (double.TryParse(timeStr[1], out var time))
                {
                    var user = _userRepository.GetItem(z => z.TelegramUserId == message.Chat.Id);
                    user.TimeToCheckBittrix = time;
                    _userRepository.Update(user,z=>z.TelegramUserId==message.Chat.Id);
                    await _client.SendTextMessageAsync(message.Chat.Id, "Success");
                    return;
                }
            await _client.SendTextMessageAsync(message.Chat.Id, "Not successful \nExample /bittrixTimeCompare 7");
        }
    }
}