using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.Commands.Alerts
{
    public class DeleteAlertCommand:ITelegramCommand
    {
        public string Name => "deletealert";
        public bool IsProtected => true;
        private IRepository<Alert> _alertRepository;
        private ITelegramBotClient _client;

        public DeleteAlertCommand(IRepository<Alert> repository, ITelegramBotClient client)
        {
            _client = client;
            _alertRepository = repository;
        }

        public async Task ExecuteAsync(Message message)
        {
            var splitStr = message.Text.Split(' ');
            if (splitStr.Length != 2)
            {
                await _client.SendTextMessageAsync(message.Chat.Id, $"Not successful \nExample /{Name} BTC");
                return;
            }
            var currency = splitStr[1].ToUpper();
            var alert = _alertRepository.GetItem(z => z.UserId == message.Chat.Id && currency == z.NameCurrency);
            if (alert == null)
            {
                await _client.SendTextMessageAsync(message.Chat.Id, "Alert not found");
                return;
            }
            _alertRepository.Delete(z=>z.UserId == message.Chat.Id && currency == z.NameCurrency);
            await _client.SendTextMessageAsync(message.Chat.Id, "Success");
        }
    }
}