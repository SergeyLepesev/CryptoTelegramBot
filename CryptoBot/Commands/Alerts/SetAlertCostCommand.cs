using System;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.Commands.Alerts
{
    public class SetAlertCostCommand : ITelegramCommand
    {
        public string Name => "setalertcost";
        public bool IsProtected => true;

        private ITelegramBotClient _client;
        private IRepository<Alert> _alertRepository;

        public SetAlertCostCommand(ITelegramBotClient client, IRepository<Alert> alertRepository)
        {
            _client = client;
            _alertRepository = alertRepository;
        }

        public async Task ExecuteAsync(Message message)
        {
            var allAlert = _alertRepository.GetItems(z => z.UserId == message.Chat.Id);
            var splitStr = message.Text.TrimEnd().Split(' ');
            string cost;
            try
            {
                cost = TakeNumber(splitStr[2]);

            }
            catch (Exception e)
            {
                await _client.SendTextMessageAsync(message.Chat.Id,
                    "Not successful \nExample /setalertcost BTC 243");
                return;
            }
            if (splitStr.Length != 3 || splitStr[1].Length > 9 || cost == null)
            {
                await _client.SendTextMessageAsync(message.Chat.Id,
                    "Not successful \nExample /setalertcost BTC 243");
                return;
            }
            string pair = splitStr[1];//здесь делай ToUpper()
            var alert = allAlert?.FirstOrDefault(z => string.Equals(z.NameCurrency, pair));
            if (allAlert == null || alert == null)
                _alertRepository.Create(new Alert{NameCurrency = pair,NotifyCost = double.Parse(cost), UserId = message.Chat.Id});
            else
            {
                alert.NotifyCost = double.Parse(cost);
                _alertRepository.Update(alert,z=>z.UserId==message.Chat.Id &&
                                                 string.Equals(z.NameCurrency, pair));
            }
            await _client.SendTextMessageAsync(message.Chat.Id, "Success");
        }

        private string TakeNumber(string str)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }
    }
}