using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceTextBot.Configuration;
using VoiceTextBot.Controllers;
using VoiceTextBot.Services;

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
        var appSettings = BuildAppSettings();
        services.AddSingleton(appSettings);
        services.AddSingleton<IStorage, MemoryStorage>();
        services.AddSingleton<IFileHandler, AudioFileHandler>();
        services.AddTransient<DefaultMessageController>();
        services.AddTransient<TextMessageController>();
        services.AddTransient<VoiceMessageController>();
        services.AddTransient<InlineKeyboardController>();
        services.AddSingleton<ITelegramBotClient>(provider =>
            new TelegramBotClient(appSettings.BotToken));
        services.AddHostedService<Bot>();
    }

    static AppSettings BuildAppSettings()
    {
        return new AppSettings()
        {
            DownloadsFolder = "/Users/a.chaturov/",
            BotToken = "***",
            AudioFileName = "audio",
            InputAudioFormat = "ogg",
            OutputAudioFormat = "wav",
            InputBitrate = 48
        };
    }
}