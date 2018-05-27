using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.Commands;
using CryptoBot.Data;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = CryptoBot.Models.DataBase.User;

namespace CryptoBot.Services
{
    public class TelegramCommandService
    {
        private readonly IEnumerable<ITelegramCommand> _commands;
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<TelegramCommandService> _logger;
        private readonly ITelegramBotClient _client;

        public TelegramCommandService(
            IEnumerable<ITelegramCommand> commands,
            IRepository<User> userRepository,
            ILogger<TelegramCommandService> logger,
            ITelegramBotClient client
        )
        {
            _commands = commands;
            _userRepository = userRepository;
            _logger = logger;
            _client = client;
        }

        public async Task Process(Message message)
        {
            var user = _userRepository
                .GetItem(x => x.TelegramUserId == message.Chat.Id);
            
            var command = _commands
                .SingleOrDefault(x =>  message.Text.Contains($"/{x.Name}"));
            
            
            
            if (command != null)
            {
                if (command.IsProtected && user == null)
                {
                    await _client.SendTextMessageAsync(message.Chat.Id, "Sign in please");
                    return;
                }
                _logger.LogInformation("Process command", command.Name, message.Chat.Id);

                await command.ExecuteAsync(message);
            }
            else if (!message.Text.Contains("/"))
            {
                if (user == null)
                {
                    await _client.SendTextMessageAsync(message.Chat.Id, "Sign in please");
                    return;
                }
                _logger.LogInformation("Process command", "information", message.Chat.Id);

                _commands.FirstOrDefault(z => z.Name == InformationCommand.NameCommand)?.ExecuteAsync(message);
            }
            else
            {
                _logger.LogInformation("Command not found", message.Chat.Id);
                await _client.SendTextMessageAsync(message.Chat.Id, "Command not found");
            }

        }

        public async Task Process(Update update)
        {
            var callbackCommand = update.CallbackQuery.Data;
            var user = _userRepository
                .GetItems(z => z.TelegramUserId == update.CallbackQuery.Message.Chat.Id);
            var command = _commands
                .SingleOrDefault(
                    z => (user != null || z.IsProtected == false) && callbackCommand.Contains($"/{z.Name}"));
            if (command != null)
            {
                _logger.LogInformation("Process command", command.Name, update.CallbackQuery.Message.Chat.Id);
                await command.ExecuteAsync(update.CallbackQuery.Message);
            }
        }
        
    }
}