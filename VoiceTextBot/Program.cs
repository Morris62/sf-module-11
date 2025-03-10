﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace VoiceTextBot;

internal class Program
{
    private static async Task Main()
    {
        //Console.OutputEncoding = Encoding.Unicode;

        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services))
            .UseConsoleLifetime()
            .Build();

        Console.WriteLine("Сервис запущен");
        await host.RunAsync();
        Console.WriteLine("Сервис остановлен");
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ITelegramBotClient>(provider =>
            new TelegramBotClient(""));
        services.AddHostedService<Bot>();
    }
}