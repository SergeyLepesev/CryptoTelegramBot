using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using CryptoBot.Services.Stores;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.Commands.Alerts
{
    public class SetAlertCommand : ITelegramCommand
    {
        public string Name => "alert";
        public bool IsProtected => true;
        private IRepository<Alert> _alertRepository;
        private ITelegramBotClient _client;
        private ILogger<SetAlertCommand> _logger;

        public SetAlertCommand(IRepository<Alert> repository, ITelegramBotClient client, ILogger<SetAlertCommand> logger)
        {
            _client = client;
            _alertRepository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync(Message message)
        {
            try
            {
                var splitStr = message.Text.Split(' ');
                var currency = splitStr[1].ToUpper();
                var value = splitStr[2].Replace(',','.');
                string sign = "";
                double valueParsed;
                if (value.Contains("+") || value.Contains("-"))
                {
                    valueParsed = double.Parse(value.Substring(1, value.Length - 2),CultureInfo.InvariantCulture);
                    sign += value[0];
                }
                else if (value.Contains("%"))
                {
                    valueParsed = double.Parse(value.Substring(0, value.Length - 1),CultureInfo.InvariantCulture);
                }
                else
                    valueParsed = double.Parse(value.Substring(0, value.Length), CultureInfo.InvariantCulture);

                var alert = _alertRepository.GetItem(z => z.UserId == message.Chat.Id && currency == z.NameCurrency);

                if (value.Contains("%"))
                {
                    var lastCoinMarcet = CurrencyInfoStore.GetLastCoinMarket()
                        .FirstOrDefault(z => z.FirstCurrency == currency);
                    if (lastCoinMarcet == null)
                    {
                        await _client.SendTextMessageAsync(message.Chat.Id, "Currency not found");
                        return;
                    }
                    if (alert == null)
                        _alertRepository.Create(new Alert
                        {
                            NameCurrency = currency,
                            PercentNotify = sign + valueParsed.ToString(CultureInfo.InvariantCulture),
                            UserId = message.Chat.Id,
                            Cost = lastCoinMarcet.Last
                        });
                    else
                    {
                        alert.PercentNotify = sign + valueParsed.ToString(CultureInfo.InvariantCulture);
                        alert.Cost = lastCoinMarcet.Last;
                        _alertRepository.Update(alert, z => z.UserId == message.Chat.Id && currency == z.NameCurrency);
                    }
                }
                else
                {
                    if (alert == null)
                        _alertRepository.Create(new Alert
                        {
                            NameCurrency = currency,
                            NotifyCost = valueParsed,
                            UserId = message.Chat.Id
                        });
                    else
                    {
                        alert.NotifyCost = valueParsed;
                        _alertRepository.Update(alert, z => z.UserId == message.Chat.Id && currency == z.NameCurrency);
                    }
                }
                await _client.SendTextMessageAsync(message.Chat.Id, "Success");
            }
            catch (Exception e)
            {
                await NotSuccess(message);
                _logger.LogError("Exeption in SetAlertCommand",e.Message);
            }
        }

        public async Task NotSuccess(Message message)
        {
            await _client.SendTextMessageAsync(message.Chat.Id, $"Not successful \nExample /{Name} BTC 243/+10%");
        }
    }
}