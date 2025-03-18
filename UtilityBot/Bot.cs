using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UtilityBot;

public class Bot(ITelegramBotClient telegramBotClient) : BackgroundService
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramBotClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions(),
            stoppingToken);

        Console.WriteLine("Бот запущен");

        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await botClient.SendMessage(update.Message.Chat.Id, "Вы нажали кнопку",
                cancellationToken: cancellationToken);
            return;
        }

        if (update.Type == UpdateType.Message)
            await botClient.SendMessage(update.Message.Chat.Id,
                $"Длина сообщения {update.Message.Text?.Length} символов",
                cancellationToken: cancellationToken);
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);

        Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
        Thread.Sleep(10_000);

        return Task.CompletedTask;
    }
}