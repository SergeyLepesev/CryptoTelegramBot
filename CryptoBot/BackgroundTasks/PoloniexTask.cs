using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using CryptoBot.Services;
using CryptoBot.Services.Stores;
using Microsoft.Extensions.Logging;

namespace CryptoBot.BackgroundTasks
{
    public class PoloniexTask : IScheduledTask
    {
        private readonly HttpClientWrapper _client;
        private readonly CurrencyService _currencyService;
        private readonly ILogger<PoloniexTask> _logger;
        private readonly Notifyer _notifyer;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Alert> _alertRepository;

        public PoloniexTask(IRepository<User> userRepository, 
                            IRepository<Alert> alertRepository, 
                            Notifyer notifyer, 
                            HttpClientWrapper client,
                            CurrencyService currencyService, 
                            ILogger<PoloniexTask> logger)
        {
            _userRepository = userRepository;
            _alertRepository = alertRepository;
            _notifyer = notifyer;
            _client = client;
            _currencyService = currencyService;
            _logger = logger;
        }

        public string Schedule => (5 * 60 * 1000).ToString();

        public async Task Invoke(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Invoked timerpoloniex");
            try
            {
                _logger.LogInformation(NoticeStore.NoticesPoloniex.Count.ToString());
                _logger.LogInformation(CurrencyInfoStore.Poloniexes.Count.ToString());

                NoticeStore.ClearPoloniex();

                var users = _userRepository.GetItems(x => x.NotifyPoloniex).ToList();

                var currencyPairs = await _client.GetPoloniex();

                CurrencyInfoStore.AddPoloniex(currencyPairs);

                var notifications = _currencyService
                    .CreateNotifications(users, CurrencyInfoStore.Poloniexes, 1, _alertRepository);

                await _notifyer.Notify(notifications);
            }
            catch (Exception e)
            {
                _logger.LogError("Timer poloniex", e.Message);
            }
        }
    }
}