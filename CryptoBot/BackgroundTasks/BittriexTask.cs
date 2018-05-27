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
    public class BittriexTask : IScheduledTask
    {
        private readonly HttpClientWrapper _client;
        private readonly CurrencyService _currencyService;
        private readonly IRepository<Alert> _alertRepository;
        private readonly ILogger<BittriexTask> _logger;
        private readonly Notifyer _notifyer;
        private readonly IRepository<User> _userRepository;

        public BittriexTask(
            IRepository<User> userRepository,
            Notifyer notifyer,
            HttpClientWrapper client,
            ILogger<BittriexTask> logger,
            CurrencyService currencyService,
            IRepository<Alert> alertRepository)
        {
            _userRepository = userRepository;
            _notifyer = notifyer;
            _client = client;
            _logger = logger;
            _currencyService = currencyService;
            _alertRepository = alertRepository;
        }

        public string Schedule => (10 * 1000).ToString();

        public async Task Invoke(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Invoked timerbittrex");
            try
            {
                _logger.LogInformation(NoticeStore.NoticesBittrix.Count.ToString());
                _logger.LogInformation(CurrencyInfoStore.Bittrixes.Count.ToString());

                NoticeStore.ClearBittrix();

                var users = _userRepository.GetItems(x => x.NotifyBittrix).ToList();
                var currencyPairs = await _client.GetBittrix();

                CurrencyInfoStore.AddBittrix(currencyPairs);

                var notifications =
                    _currencyService.CreateNotifications(users, CurrencyInfoStore.Bittrixes, 0, _alertRepository);

                await _notifyer.Notify(notifications);
            }
            catch (Exception e)
            {
                _logger.LogError("Timer bittriex", e.Message);
            }
        }
    }
}