using CryptoBot.Data;
using CryptoBot.Models.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace CryptoBot.Controllers
{
	/// <summary>
	/// Expecting url as site/?userId=123
	/// </summary>
	public class AsicController : Controller
	{
		private readonly IRepository<Asic> _asicRepository;

		public AsicController(IRepository<Asic> repository)
		{
			_asicRepository = repository;
		}

		[HttpPost]
		public IActionResult Settings(Asic asic, [FromQuery]long userId)
		{
			var oldAsic = _asicRepository.GetItem(z => z.TelegramUserId == asic.TelegramUserId);

			if (oldAsic == null)
			{
				_asicRepository.Create(asic);
			}
			else
			{
				_asicRepository.Update(asic, z => z.TelegramUserId == asic.TelegramUserId);
			}
			return Ok();
		}
	}
}