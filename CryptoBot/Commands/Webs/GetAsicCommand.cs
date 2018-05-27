using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.Commands.Webs
{
    public class GetAsicCommand:ITelegramCommand
    {
        public string Name => "getasic";
        public bool IsProtected => true;
        private IRepository<Asic> _asicRepository;
        private ITelegramBotClient _client;

        public GetAsicCommand(IRepository<Asic> repository, ITelegramBotClient client)
        {
            _client = client;
            _asicRepository = repository;
        }
        public async Task ExecuteAsync(Message message)
        {
            var asic = _asicRepository.GetItem(z => z.TelegramUserId == message.Chat.Id);
            await _client.SendTextMessageAsync(message.Chat.Id, asic.Quark_HR.ToString());
        }
    }
}