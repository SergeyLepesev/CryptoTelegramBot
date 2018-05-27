using System;
using System.Collections.Generic;
using System.Linq;
using CryptoBot.Models;

namespace CryptoBot.Services.Stores
{
    public static class NoticeStore
    {
        private static List<Notice> _noticesBittrix = new List<Notice>();
        private static List<Notice> _noticesPoloniex = new List<Notice>();
        private static List<Notice> _noticesCoinMarket = new List<Notice>();

        public static List<Notice> NoticesBittrix => _noticesBittrix;
        public static List<Notice> NoticesPoloniex => _noticesPoloniex;
        public static List<Notice> NoticesCoinMarket => _noticesCoinMarket;

        public static void AddBittrix(Notice notice)
        {
            _noticesBittrix.Add(notice);
        }

        public static void ClearBittrix()
        {
            _noticesBittrix = _noticesBittrix
                .Where(x => (DateTime.UtcNow - x.Time).TotalSeconds <= 4 * 60)
                .ToList();
        }
        public static void AddPoloniex(Notice notice)
        {
            _noticesPoloniex.Add(notice);
        }

        public static void ClearPoloniex()
        {
            _noticesPoloniex = _noticesPoloniex
                .Where(x => (DateTime.UtcNow - x.Time).TotalSeconds <= 4 * 60)
                .ToList();
            //_noticesPoloniex = new List<Notice>();
        }

        public static void AddCoinMarket(Notice notice)
        {
            _noticesCoinMarket.Add(notice);
        }

        public static void ClearCoinMarket()
        {
            _noticesCoinMarket = _noticesCoinMarket
                .Where(x => (DateTime.UtcNow - x.Time).TotalSeconds <= 4 * 60)
                .ToList();
            //_noticesCoinMarket = new List<Notice>();
        }
        public static bool IsNotifiedPoloniex(long userId, string name, TypeNotice type)
        {
            return _noticesPoloniex.Any(x => x.Name == name && x.UserId == userId && x.Type == type);
        }

        public static bool IsNotifiedBittrix(long userId, string name, TypeNotice type)
        {
            return _noticesBittrix.Any(x => x.Name == name && x.UserId == userId && x.Type == type);
        }

        public static bool IsNotifiedCoinMarket(long userId, string name, TypeNotice type)
        {
            return _noticesCoinMarket.Any(x => x.Name == name && x.UserId == userId && x.Type==type);
        }
    }
}