﻿using System.Threading.Tasks;
using CryptoBot.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CryptoBot.Models.DataBase.User;

namespace CryptoBot.Commands
{
    public class NotifyCoinMarketTrueCommand:ITelegramCommand
    {
        public string Name => NameCommand;
        public static string NameCommand = "notcoinmarkettrue";
        public bool IsProtected => true;

        private IRepository<User> _userRepository;
        private ITelegramBotClient _client;

        public NotifyCoinMarketTrueCommand(ITelegramBotClient client, IRepository<User> repository)
        {
            _client = client;
            _userRepository = repository;
        }

        public async Task ExecuteAsync(Message message)
        {
            var user = _userRepository.GetItem(z => z.TelegramUserId == message.Chat.Id);
            user.NotifyCoinMarket = true;
            _userRepository.Update(user,z=>z.TelegramUserId==message.Chat.Id);
            await _client.SendTextMessageAsync(message.Chat.Id, "Success");
        }
    }
}