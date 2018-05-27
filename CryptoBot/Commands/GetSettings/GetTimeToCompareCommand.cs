using System.Threading.Tasks;
using CryptoBot.Data;
using Microsoft.Azure.KeyVault.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CryptoBot.Models.DataBase.User;

namespace CryptoBot.Commands.GetSettings
{
    public class GetTimeToCompareCommand:ITelegramCommand
    {
        public string Name => "gettimesettings";
        public bool IsProtected => true;

        private readonly ITelegramBotClient _client;
        private readonly IRepository<User> _userRepository;

        public GetTimeToCompareCommand(ITelegramBotClient client, IRepository<User> repository)
        {
            _client = client;
            _userRepository = repository;
        }

        public async Task ExecuteAsync(Message message)
        {
            var user = _userRepository.GetItem(z => z.TelegramUserId == message.Chat.Id);
            string report = "Time to compare currency\n";
            report += $"Bittrix time = {user.TimeToCheckBittrix} min\n";
            report += $"Poloniex time = {user.TimeToCheckPoloniex} min";
            await _client.SendTextMessageAsync(message.Chat.Id, report);
        }
    }
}