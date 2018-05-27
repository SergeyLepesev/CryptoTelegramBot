using System.Threading.Tasks;
using CryptoBot.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CryptoBot.Models.DataBase.User;

namespace CryptoBot.Commands.PercentChange
{
    public class PoloniexCostChangeCommand:ITelegramCommand
    {
        public string Name => "poloniexcostchange";
        public bool IsProtected => true;
        private readonly ITelegramBotClient _client;
        private readonly IRepository<User> _userRepository;

        public PoloniexCostChangeCommand(ITelegramBotClient client, IRepository<User> repository)
        {
            _client = client;
            _userRepository = repository;
        }

        public async Task ExecuteAsync(Message message)
        {
            var percentStr = message.Text.TrimEnd().Split(' ');
            if (percentStr.Length > 1)
                if (double.TryParse(percentStr[1], out var percent))
                {
                    var user = _userRepository.GetItem(z => z.TelegramUserId == message.Chat.Id);
                    user.PoloniexPercentChangeCost = percent;
                    _userRepository.Update(user,z=>z.TelegramUserId==message.Chat.Id);
                    await _client.SendTextMessageAsync(message.Chat.Id, "Success");
                    return;
                }
            await _client.SendTextMessageAsync(message.Chat.Id, "Not successful \nExample /poloniexCostChange 14,88");
        }
    }
}