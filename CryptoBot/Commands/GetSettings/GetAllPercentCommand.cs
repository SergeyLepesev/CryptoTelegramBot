using System.Threading.Tasks;
using CryptoBot.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CryptoBot.Models.DataBase.User;

namespace CryptoBot.Commands.GetSettings
{
    public class GetAllPercentCommand:ITelegramCommand
    {
        public string Name => "getpercent";
        public bool IsProtected => true;

        private readonly ITelegramBotClient _client;
        private readonly IRepository<User> _userRepository;

        public GetAllPercentCommand(ITelegramBotClient client, IRepository<User> repository)
        {
            _client = client;
            _userRepository = repository;
        }

        public async Task ExecuteAsync(Message message)
        {
            var user = _userRepository.GetItem(z => z.TelegramUserId == message.Chat.Id);
            string report = "You settings percent\n";
            report += $"Bittrix volume percent = {user.BittrixPercentVolume}%\n";
            report += $"Bittrix cost percent = {user.BittrixPercentChangeCost}%\n";
            report += $"Poloniex volume percent = {user.PoloniexPercentVolume}%\n";
            report += $"Poloniex cost percent = {user.PoloniexPercentChangeCost}%";
            await _client.SendTextMessageAsync(message.Chat.Id, report);
        }
    }
}