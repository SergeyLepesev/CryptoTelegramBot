using System.Threading.Tasks;
using CryptoBot.Data;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace CryptoBot.Commands.HardwareCost
{
    public class D3Command : ITelegramCommand
    {
        public string Name => "d3";
        public bool IsProtected => true;

        private ITelegramBotClient _client;
        private IRepository<Models.DataBase.User> _user;

        public D3Command(ITelegramBotClient client, IRepository<Models.DataBase.User> user)
        {
            _client = client;
            _user = user;
        }

        public async Task ExecuteAsync(Message message)
        {
            var cost = message.Text.Replace($"/{Name}", string.Empty).Trim();
            if (double.TryParse(cost, out var costResult))
            {
                var hardWare = _user.GetItem(z => z.TelegramUserId == message.Chat.Id);
                hardWare.D3Cost = costResult;
                _user.Update(hardWare, z => z.TelegramUserId == message.Chat.Id);
                await _client.SendTextMessageAsync(message.Chat.Id, "Success");
            }
            else
            {
                await _client.SendTextMessageAsync(message.Chat.Id, "Invalid cost");
            }
        }
    }
}