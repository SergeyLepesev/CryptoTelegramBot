using System;
using CryptoBot.BackgroundTasks;
using CryptoBot.Commands;
using CryptoBot.Commands.Alerts;
using CryptoBot.Commands.GetSettings;
using CryptoBot.Commands.HardwareCost;
using CryptoBot.Commands.PercentChange;
using CryptoBot.Commands.TimeToCompare;
using CryptoBot.Commands.UnconfirmedTransactions;
using CryptoBot.Data;
using CryptoBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace CryptoBot
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddCors()
                .AddJsonFormatters();

            var token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
            var domain = Environment.GetEnvironmentVariable("HEROKU_URL");
            
            var telegramBotClient = new TelegramBotClient(token);

            var isDevelopment = !string.IsNullOrEmpty(domain);

            var webhookUrl = isDevelopment ? $"{domain}/api/webhook" : string.Empty;
            telegramBotClient.SetWebhookAsync(webhookUrl);

            services.AddSingleton<ITelegramBotClient>(telegramBotClient);
            services.AddSingleton<HttpClientWrapper>();
            services.AddSingleton<Notifyer>();
            services.AddSingleton<DataContext>();
            services.AddSingleton<CurrencyService>();
            services.AddSingleton<ProfitMineService>();

            services.AddSingleton(typeof(IRepository<>), typeof(MongoDbRepository<>));

            // Add scheduled tasks & scheduler
            services.AddSingleton<IScheduledTask, PoloniexTask>();
            services.AddSingleton<IScheduledTask, BittriexTask>();
            services.AddSingleton<IScheduledTask, CoinMarketTask>();
            services.AddSingleton<IHostedService, SchedulerHostedService>();

            // Telegram commands
            services.AddScoped<ITelegramCommand, HelpCommand>();//help
            services.AddScoped<ITelegramCommand, LoginCommand>();//login
            services.AddScoped<ITelegramCommand, NotifyPoloniexCommand>();//notifypoloniex
            services.AddScoped<ITelegramCommand, NotifyPoloniexTrueCommand>();//notpolTrue
            services.AddScoped<ITelegramCommand, NotifyPoloniexFalseCommand>();//notpolFalse
            services.AddScoped<ITelegramCommand, NotifyBittrixCommand>();//notifybittrix
            services.AddScoped<ITelegramCommand, NotifyBittrixTrueCommand>();//notbitTrue
            services.AddScoped<ITelegramCommand, NotifyBittrixFalseCommand>();//notbitFalse
            services.AddScoped<ITelegramCommand, PoloniexVolumeChangeCommand>();//poloniexvolumechange
            services.AddScoped<ITelegramCommand, BittrixVolumeChangeCommand>();//bittrixvolumechange
            services.AddScoped<ITelegramCommand, PoloniexCostChangeCommand>();//poloniexcostchange
            services.AddScoped<ITelegramCommand, BittrixCostChangeCommand>();//bittrixcostchange
            services.AddScoped<ITelegramCommand, TimeToCompareBittrixCommand>();//bittrixtimecompare
            services.AddScoped<ITelegramCommand, TimeToComparePoloniexCommand>();//poloniextimecompare
            services.AddScoped<ITelegramCommand, GetTimeToCompareCommand>();//gettimesettings
            services.AddScoped<ITelegramCommand, GetAllPercentCommand>();//getpercent

            services.AddScoped<ITelegramCommand, InformationCommand>();//Information
            services.AddScoped<ITelegramCommand, NotifyCoinMarketCommand>();//notifycoinmarket
            services.AddScoped<ITelegramCommand, NotifyCoinMarketFalseCommand>();//notcoinmarketfalse
            services.AddScoped<ITelegramCommand, NotifyCoinMarketTrueCommand>();//notcoinmarkettrue
            services.AddScoped<ITelegramCommand, SetAlertCommand>();//alert
            services.AddScoped<ITelegramCommand, GetAlertCommand>();//getalert
            services.AddScoped<ITelegramCommand, DeleteAlertCommand>();//deletealert
            services.AddScoped<ITelegramCommand, MainCommand>();//main
            services.AddScoped<ITelegramCommand, UnconfirmedBtcCommand>();//unconfirmed transactions BTC
            //services.AddScoped<ITelegramCommand, AsicCommand>();

            //services.AddScoped<ITelegramCommand, GetAsicCommand>();

            services.AddScoped<ITelegramCommand, ProfitCommand>();//profit
            services.AddScoped<ITelegramCommand, S9Command>();//s9
            services.AddScoped<ITelegramCommand, D3Command>();//d3
            services.AddScoped<ITelegramCommand, L3PlusCommand>();//l3
            services.AddScoped<ITelegramCommand, B8Command>();//b8
            services.AddScoped<ITelegramCommand, Nvidia1070>();//n1070
            services.AddScoped<ITelegramCommand, Nvidia1080Ti>();//n1080ti
            services.AddScoped<ITelegramCommand, HardwareSettingsCommand>();//hw


            services.AddScoped<TelegramCommandService>();
        }

        public void Configure(IApplicationBuilder app)
        {
	        app
				.UseDefaultFiles()
				.UseStaticFiles()
				.UseMvcWithDefaultRoute()
				.UseDeveloperExceptionPage()
				.UseDatabaseErrorPage();
        }
    }
}