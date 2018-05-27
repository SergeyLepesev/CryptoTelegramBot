using System;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.Commands.Alerts
{
    public class GetAlertCommand:ITelegramCommand
    {
        public string Name => "getalerts";
        public bool IsProtected => true;
        private IRepository<Alert> _alertRepository;
        private ITelegramBotClient _client;

        public GetAlertCommand(IRepository<Alert> repository, ITelegramBotClient client)
        {
            _alertRepository = repository;
            _client = client;
        }

        public async Task ExecuteAsync(Message message)
        {
            var allAllert = _alertRepository.GetItems(z => z.UserId == message.Chat.Id);
            if (allAllert == null || !allAllert.Any())
            {
                await _client.SendTextMessageAsync(message.Chat.Id, "No one installed Alert");
                return;
            }
            var report = String.Join(Environment.NewLine, allAllert);
            await _client.SendTextMessageAsync(message.Chat.Id, report);
        }
    }
}