using System.Threading.Tasks;
using CryptoBot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace CryptoBot.Controllers
{
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly TelegramCommandService _telegramCommandService;

        public WebhookController(TelegramCommandService telegramCommandService)
        {
            _telegramCommandService = telegramCommandService;
        }

        [HttpPost]
        public async Task<IActionResult> Process([FromBody] Update update)
        {
            if (update.Message != null)
                await _telegramCommandService.Process(update.Message);
            else if(update.CallbackQuery != null)
                await _telegramCommandService.Process(update);
            return Ok();
        }
    }
}