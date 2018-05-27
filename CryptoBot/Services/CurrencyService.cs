using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CryptoBot.Data;
using CryptoBot.Models;
using CryptoBot.Models.API;
using CryptoBot.Models.DataBase;
using CryptoBot.Services.Stores;

namespace CryptoBot.Services
{
    public class CurrencyService
    {
        public List<Notification> CreateNotifications(IEnumerable<User> users, List<IEnumerable<CurrencyPair>> currencyPairs, int type, IRepository<Alert> repository)
        {
            var notifications = new List<Notification>();

            foreach (var user in users)
            {
                var reports = new List<Report>();
                switch (type)
                {
                    case 0:
                        {
                            reports = CreateBittrixReports(user, currencyPairs);
                            break;
                        }
                    case 1:
                        {
                            reports = CreatePoloniexReports(user, currencyPairs);
                            break;
                        }
                    case 2:
                        {
                            reports = CreateCoinMarketRepots(user, currencyPairs, repository);
                            break;
                        }
                }

                if (reports.Any())
                {
                    notifications.Add(new Notification(user.TelegramUserId, reports));
                }
            }

            return notifications;
        }

        private List<Report> CreateCoinMarketRepots(User user, List<IEnumerable<CurrencyPair>> currencyPairs,
            IRepository<Alert> _alertRepository)
        {
            var reports = new List<Report>();
            if (currencyPairs.Count == 1)
            {
                return reports;
            }
            var alerts = _alertRepository.GetItems(x => x.UserId == user.TelegramUserId);
            var oldValues = currencyPairs[currencyPairs.Count - 2];
            var newValues = currencyPairs.Last();

            foreach (var value in newValues)
            {
                if (
                    alerts.Any(
                        x => string.Equals(x.NameCurrency, value.CurrencyPairName, StringComparison.OrdinalIgnoreCase)))
                {
                    var alert = alerts.FirstOrDefault(x => x.NameCurrency == value.CurrencyPairName);
                    var percent = alert.PercentNotify;
                    var cost = alert.NotifyCost;
                    var oldCurrnecy = oldValues.FirstOrDefault(x => x.CurrencyPairName == value.CurrencyPairName);
                    if (!string.IsNullOrEmpty(percent))
                    {
                        var report = CheckPercent(alert, value);
                        if (report != null &&
                            !NoticeStore.IsNotifiedCoinMarket(user.TelegramUserId, value.CurrencyPairName,
                                TypeNotice.Percent))
                        {
                            reports.Add(report);
                            alert.PercentNotify = null;
                        }
                    }
                    if (cost.HasValue)
                    {
                        var report = CheckCost(value, oldCurrnecy, cost.Value);
                        if (report != null &&
                            !NoticeStore.IsNotifiedCoinMarket(user.TelegramUserId, value.CurrencyPairName,
                                TypeNotice.Cost))
                        {
                            reports.Add(report);
                            alert.NotifyCost = null;
                            
                        }
                    }
                    if (alert.PercentNotify == null && alert.NotifyCost == null)
                        _alertRepository.Delete(z=>z.UserId == user.TelegramUserId && z.NameCurrency == value.FirstCurrency);
                    else
                        _alertRepository.Update(alert, x => x.UserId == user.TelegramUserId && x.NameCurrency == value.FirstCurrency);

                }
                if (reports.Any(x => x.CurrencyPair == value.CurrencyPairName && x.Type == TypeNotice.Percent))
                    NoticeStore.AddCoinMarket(new Notice(user.TelegramUserId, value.CurrencyPairName, TypeNotice.Percent));
                if (reports.Any(x => x.CurrencyPair == value.CurrencyPairName && x.Type == TypeNotice.Cost))
                    NoticeStore.AddCoinMarket(new Notice(user.TelegramUserId, value.CurrencyPairName, TypeNotice.Cost));
            }
            return reports;
        }

        private Report CheckPercent(Alert alert, CurrencyPair currency)
        {
            var percent = alert.PercentNotify;
            switch (percent[0])
            {
                case '-':
                    {
                        var p = double.Parse(percent, CultureInfo.InvariantCulture);
                        if ( alert.Cost != null && 100*(1 - currency.Last / alert.Cost.Value) > p)
                            return new Report
                            {
                                Type = TypeNotice.Percent,
                                CurrencyPair = currency.CurrencyPairName,
                                Text = $"Курс {currency.CurrencyPairName} изменился на -{1 - currency.Last / alert.Cost.Value:F2}%"
                            };
                        break;
                    }
                case '+':
                    {
                        var p = double.Parse(percent.Substring(1), CultureInfo.InvariantCulture);
                        if (alert.Cost != null && 100*(currency.Last / alert.Cost.Value - 1)  > p)
                            return new Report
                            {
                                Type = TypeNotice.Percent,
                                CurrencyPair = currency.CurrencyPairName,
                                Text = $"Курс {currency.CurrencyPairName} изменился на +{currency.Last / alert.Cost.Value - 1:F2}%"
                            };
                        break;
                    }
                default:
                    {
                        var p = double.Parse(percent, CultureInfo.InvariantCulture);
                        if (alert.Cost != null)
                        {
                            var abs = Math.Abs(currency.Last / alert.Cost.Value - 1)*100;
                            var sign = currency.Last / alert.Cost.Value - 1 > 0 ? '+' : '-';
                            if (abs > p)
                                return new Report
                                {
                                    Type = TypeNotice.Percent,
                                    CurrencyPair = currency.CurrencyPairName,
                                    Text = $"Курс {currency.CurrencyPairName} изменился на {sign}{abs:F2}%"
                                };
                        }
                        break;
                    }
            }
            return null;
        }

        private Report CheckCost(CurrencyPair newValue, CurrencyPair oldValue, double cost)
        {
            if ((newValue.Last - cost) * (oldValue.Last - cost) < 0)
            {
                return new Report
                {
                    Type = TypeNotice.Cost,
                    CurrencyPair = newValue.CurrencyPairName,
                    Text = $"{newValue.CurrencyPairName} преодолел порог в {cost:F2} usd",
                };
            }
            return null;
        }
        private List<Report> CreateBittrixReports(User user, List<IEnumerable<CurrencyPair>> currencyPairs)
        {
            var userTimeToCheckBittrix = user.TimeToCheckBittrix;
            var userNumberChat = user.TelegramUserId;
            var userBittrixPercentChangeCost = user.BittrixPercentChangeCost;
            var userBittrixPercentVolume = user.BittrixPercentVolume;

            var reports = new List<Report>();
            int ind = (int)Math.Round(userTimeToCheckBittrix * 5);
            var oldValues = currencyPairs.Count <= ind
                ? currencyPairs[0]
                : currencyPairs[currencyPairs.Count - ind - 1];

            var newValues = currencyPairs.Last().ToList();

            foreach (var newValue in newValues)
            {
                var oldValue = oldValues
                    .Where(x => x.CurrencyPairName == newValue.CurrencyPairName)
                    .ToList();

                if (oldValue.Any())
                {
                    var percent = (newValue.Last / oldValue.First().Last - 1) * 100;
                    if (Math.Abs(percent) > userBittrixPercentChangeCost)
                    {
                        var sign = percent >= 0 ? '+' : '-';
                        var value = Math.Abs(percent);
                        if (!NoticeStore.IsNotifiedBittrix(user.TelegramUserId, newValue.CurrencyPairName, TypeNotice.Percent))
                        {
                            reports.Add(new Report
                            {
                                Type = TypeNotice.Percent,
                                Text = $"Курс {newValue.CurrencyPairName} изменился на {sign}{value:F2}%",
                                CurrencyPair = newValue.CurrencyPairName
                            });
                        }
                    }
                    percent = (newValue.BaseVolume / oldValue.First().BaseVolume - 1) * 100;
                    if (percent > userBittrixPercentVolume)
                    {
                        var sign = percent >= 0 ? '+' : '-';
                        var value = Math.Abs(percent);
                        if (!NoticeStore.IsNotifiedBittrix(user.TelegramUserId, newValue.CurrencyPairName, TypeNotice.Volume))
                        {
                            reports.Add(new Report
                            {
                                Type = TypeNotice.Volume,
                                Text = $"Объем {newValue.CurrencyPairName} изменился на {sign}{value:F2}%",
                                CurrencyPair = newValue.CurrencyPairName
                            });
                        }
                    }

                    if (reports.Any(x => x.CurrencyPair == newValue.CurrencyPairName && x.Type==TypeNotice.Percent))
                        NoticeStore.AddBittrix(new Notice(userNumberChat, newValue.CurrencyPairName,TypeNotice.Percent));
                    if (reports.Any(x => x.CurrencyPair == newValue.CurrencyPairName && x.Type == TypeNotice.Volume))
                        NoticeStore.AddBittrix(new Notice(userNumberChat, newValue.CurrencyPairName, TypeNotice.Volume));
                }
            }
            return reports;
        }

        private List<Report> CreatePoloniexReports(User user, List<IEnumerable<CurrencyPair>> currencyPairs)
        {
            var userTimeToCheckPoloniex = user.TimeToCheckPoloniex;
            var userNumberChat = user.TelegramUserId;
            var userPoloniexPercentChangeCost = user.PoloniexPercentChangeCost;

            var reports = new List<Report>();
            int ind = (int)Math.Round(userTimeToCheckPoloniex / 5);
            var oldValues = currencyPairs.Count <= ind
                ? currencyPairs[0].ToList()
                : currencyPairs[currencyPairs.Count - ind - 1].ToList();

            var newValues = currencyPairs.Last().ToList();

            foreach (var newValue in newValues)
            {
                var oldValue = oldValues
                    .Where(x => x.CurrencyPairName == newValue.CurrencyPairName)
                    .ToList();
                if (oldValue.Any())
                {
                    var percent = (newValue.Last/oldValue.First().Last -1 ) * 100;
                    if (Math.Abs(percent) > userPoloniexPercentChangeCost)
                    {
                        var sign = percent >= 0 ? '+' : '-';
                        var value = Math.Abs(percent);
                        if (!NoticeStore.IsNotifiedPoloniex(user.TelegramUserId, newValue.CurrencyPairName, TypeNotice.Percent))
                        {
                            reports.Add(new Report
                            {
                                Type = TypeNotice.Percent,
                                Text = $"Курс {newValue.CurrencyPairName} изменился на {sign}{value:F2}%",
                                CurrencyPair = newValue.CurrencyPairName
                            });
                        }
                    }
                    if (oldValues.Count == newValues.Count)
                    {
                        var sortedOld = oldValues.OrderBy(x => x.BaseVolume).ToList();
                        var indOld = sortedOld.FindIndex(x=> x.CurrencyPairName == newValue.CurrencyPairName);

                        var sortedNew = newValues.OrderBy(x => x.BaseVolume).ToList();
                        var indNew = sortedNew.FindIndex(x => x.CurrencyPairName == newValue.CurrencyPairName);
                        if (!NoticeStore.IsNotifiedPoloniex(user.TelegramUserId, newValue.CurrencyPairName, TypeNotice.Volume) && indNew - indOld >= 2)
                        {
                            reports.Add(new Report
                            {
                                Type = TypeNotice.Volume,
                                Text = $"{newValue.SecondCurrency} поднялся на {indNew - indOld} поз.",
                                CurrencyPair = newValue.CurrencyPairName
                            });
                        }
                    }

                    if (reports.Any(x => x.CurrencyPair == newValue.CurrencyPairName && x.Type == TypeNotice.Percent))
                        NoticeStore.AddPoloniex(new Notice(userNumberChat, newValue.CurrencyPairName, TypeNotice.Percent));
                    if (reports.Any(x => x.CurrencyPair == newValue.CurrencyPairName && x.Type == TypeNotice.Volume))
                        NoticeStore.AddPoloniex(new Notice(userNumberChat, newValue.CurrencyPairName, TypeNotice.Volume));
                }
            }
            return reports;
        }
    }
}