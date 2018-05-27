using System;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.Commands.Alerts
{
    public class SetAlertPercentCommand : ITelegramCommand
    {
        public string Name => "setalertpercent";
        public bool IsProtected => true;

        private ITelegramBotClient _client;
        private IRepository<Alert> _alertRepository;

        public SetAlertPercentCommand(ITelegramBotClient client,IRepository<Alert> alertRepository)
        {
            _client = client;
            _alertRepository = alertRepository;
        }

        public async Task ExecuteAsync(Message message)
        {
            var allAlerts = _alertRepository.GetItems(z => z.UserId == message.Chat.Id);
            var splitStr = message.Text.TrimEnd().Split(' ');
            string percent;
            try
            {
                percent = TakeNumber(splitStr[2]);
            }
            catch (Exception e)
            {
                await _client.SendTextMessageAsync(message.Chat.Id,
                    "Not successful \nExample /setalertpercent BTC +/-/nothing 10%");
                return;
            }
            if (splitStr.Length != 3 || splitStr[1].Length > 9 || percent == null)
            {
                await _client.SendTextMessageAsync(message.Chat.Id,
                    "Not successful \nExample /setalertpercent BTC +/-/nothing 10%");
                return;
            }
            string pair = splitStr[1];
            string percentForAlert;
            var alert = allAlerts?.FirstOrDefault(z => string.Equals(z.NameCurrency, pair));
            if (splitStr[2][0] == '+' || splitStr[2][0] == '-')
            {
                percentForAlert = splitStr[2][0] + percent;
            }
            else
                percentForAlert = percent;
            if (allAlerts == null || alert == null)
                _alertRepository.Create(new Alert { NameCurrency = pair, PercentNotify = percentForAlert, UserId = message.Chat.Id });
            else
            {
                alert.PercentNotify = percentForAlert;
                _alertRepository.Update(alert,z=>z.UserId == message.Chat.Id &&
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