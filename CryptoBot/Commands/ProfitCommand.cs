using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.Data;
using CryptoBot.Models;
using CryptoBot.Services;
using CryptoBot.Services.Stores;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CryptoBot.Commands
{
    public class ProfitCommand : ITelegramCommand
    {
        public string Name => "profit";
        public bool IsProtected => true;
        private ITelegramBotClient _client;
        private readonly ProfitMineService _profitMineService;
        private readonly IRepository<Models.DataBase.User> _hardwareRepository;

        public ProfitCommand(ITelegramBotClient client,ProfitMineService profitMineService, IRepository<Models.DataBase.User> hardwareRepository)
        {
            _client = client;
            _profitMineService = profitMineService;
            _hardwareRepository = hardwareRepository;
        }

        public async Task ExecuteAsync(Message message)
        {
            //испавить этот костыль
            //if (_hardwareRepository.GetItem(x => x.TelegramUserId == message.Chat.Id) == null)
            //{
            //    _hardwareRepository.Create(new Models.DataBase.HardwareCost()
            //    {
            //        TelegramUserId = message.Chat.Id
            //    });
            //    return;
            //}
            
            var hardwareCost = _hardwareRepository.GetItem(x => x.TelegramUserId == message.Chat.Id);
            _profitMineService.SetCost(hardwareCost);
            
            List<ReportProfit> report = new List<ReportProfit>();

            var s9 = await _profitMineService.GetProfit(ProfitMinerStore.minerS9);
            report.Add(new ReportProfit()
            {
                Name = "S9",
                Profit = s9
            });
            var l3 = await _profitMineService.GetProfit(ProfitMinerStore.minerL3);
            report.Add(new ReportProfit()
            {
                Name = "L3+",
                Profit = l3
            });
            var d3 = await _profitMineService.GetProfit(ProfitMinerStore.minerD3);
            report.Add(new ReportProfit()
            {
                Name = "D3",
                Profit = d3
            });
          
            var b8 = await _profitMineService.GetProfit(ProfitMinerStore.minerB8);
            report.Add(new ReportProfit()
            {
                Name = "B8",
                Profit = b8
            });
            var n70 = await _profitMineService.GetProfit(ProfitMinerStore.miner1070);
            report.Add(new ReportProfit()
            {
                Name = "1070",
                Profit = n70
            });
            var n80ti = await _profitMineService.GetProfit(ProfitMinerStore.miner1080ti);
            report.Add(new ReportProfit()
            {
                Name = "1080ti",
                Profit = n80ti
            });
            await _client.SendTextMessageAsync(message.Chat.Id, '`'+string.Join(Environment.NewLine,report.Select(x=>x.ToString()))+'`', ParseMode.Markdown);
        }
    }
}