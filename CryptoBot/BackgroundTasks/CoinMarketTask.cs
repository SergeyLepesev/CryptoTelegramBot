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
    public class CoinMarketTask : IScheduledTask
    {
        private readonly HttpClientWrapper _client;
        private readonly CurrencyService _currencyService;
        private readonly ILogger<CoinMarketTask> _logger;
        private readonly IRepository<Alert> _alertRepository;
        private readonly Notifyer _notifyer;
        private readonly IRepository<User> _userRepository;

        public CoinMarketTask(IRepository<User> userRepository, Notifyer notifyer, HttpClientWrapper client,
            CurrencyService currencyService,
            ILogger<CoinMarketTask> logger,
            IRepository<Alert> alertRepository )
        {
            _userRepository = userRepository;
            _notifyer = notifyer;
            _client = client;
            _currencyService = currencyService;
            _logger = logger;
            _alertRepository = alertRepository;
        }

        public string Schedule => (5 * 60 * 1000).ToString();

        public async Task Invoke(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Invoked timercoinmarket");
            try
            {
                _logger.LogInformation(NoticeStore.NoticesCoinMarket.Count.ToString());
                _logger.LogInformation(CurrencyInfoStore.CoinMarkets.Count.ToString());

                NoticeStore.ClearCoinMarket();

                //_alertRepository.DeleteMany(x=> x.NotifyCost == null && string.IsNullOrEmpty(x.PercentNotify));

                var users = _userRepository.GetItems(z => z.NotifyCoinMarket).ToList(); //+

                var currencyPairs = await _client.GetCoinMarket();

                CurrencyInfoStore.AddCoinMarket(currencyPairs);

                var notifications = _currencyService
                    .CreateNotifications(users, CurrencyInfoStore.CoinMarkets, 2, _alertRepository);

                await _notifyer.Notify(notifications);
            }
            catch (Exception e)
            {
                _logger.LogError("Timer coinmarket", e.Message);
            }
        }
    }
}