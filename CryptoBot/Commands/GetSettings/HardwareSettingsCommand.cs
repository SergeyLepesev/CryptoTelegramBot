using System;
using System.Threading.Tasks;
using CryptoBot.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CryptoBot.Models.DataBase.User;

namespace CryptoBot.Commands.GetSettings
{
    public class HardwareSettingsCommand : ITelegramCommand
    {
        public string Name => "hw";
        public bool IsProtected => true;

        private ITelegramBotClient _client;
        private IRepository<User> _userRepository;

        public HardwareSettingsCommand(ITelegramBotClient client, IRepository<User> user)
        {
            _client = client;
            _userRepository = user;
        }

        public async Task ExecuteAsync(Message message)
        {
            string report = "`";
            var user = _userRepository.GetItem(z => z.TelegramUserId == message.Chat.Id);
            report += $"D3       {user.D3Cost}$" + Environment.NewLine;
            report += $"B8       {user.B8Cost}$" + Environment.NewLine;
            report += $"S9       {user.S9Cost}$" + Environment.NewLine;
            report += $"L3+      {user.L3PlusCost}$" + Environment.NewLine;
            report += $"Nv1070   {user.Nvidia1070}$" + Environment.NewLine;
            report += $"Nv1080Ti {user.Nvidia1080Ti}$`";
            await _client.SendTextMessageAsync(message.Chat.Id, report, ParseMode.Markdown);
        }
    }
}