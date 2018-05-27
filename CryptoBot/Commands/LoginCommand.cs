using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CryptoBot.Models.DataBase.User;

namespace CryptoBot.Commands
{
    public class LoginCommand : ITelegramCommand
    {
        private readonly ITelegramBotClient _client;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Models.DataBase.HardwareCost> _hwRepository;

        public LoginCommand(ITelegramBotClient client, IRepository<User> userRepository, IRepository<Models.DataBase.HardwareCost> hwRepository)
        {
            _client = client;
            _userRepository = userRepository;
            _hwRepository = hwRepository;
        }

        public string Name => "login";
        public bool IsProtected => false;

        public async Task ExecuteAsync(Message message)
        {
            // security
            if (_userRepository.GetItem(x => x.TelegramUserId == message.Chat.Id) != null)
            {
                await _client.SendTextMessageAsync(message.Chat.Id, "Invalid command!");
                return;
            }

            if (message.Text.Replace($"/{Name}", string.Empty).Trim() == "password")
            {
                _userRepository.Create(new User
                {
                    TelegramUserId = message.Chat.Id
                });

                //_hwRepository.Create(new Models.DataBase.HardwareCost()
                //{
                //    TelegramUserId = message.Chat.Id
                //});


                await _client.SendTextMessageAsync(message.Chat.Id, "Welcome!");

            }
            else
            {
                await _client.SendTextMessageAsync(message.Chat.Id, "Unknown password");
            }

        }
    }
}