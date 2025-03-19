using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Controllers;

namespace UtilityBot;

public class Bot(
    ITelegramBotClient telegramBotClient,
    DefaultMessageController defaultMessageController,
    InlineKeyboardController inlineKeyboardController,
    TextMessageController textMessageController) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        telegramBotClient.StartReceiving(
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
        switch (update.Type)
        {
            case UpdateType.CallbackQuery:
                await inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                break;
            case UpdateType.Message:
                switch (update.Message?.Type)
                {
                    case MessageType.Text:
                        await textMessageController.Handle(update.Message, cancellationToken);
                        break;
                    default:
                        await defaultMessageController.Handle(update.Message, cancellationToken);
                        break;
                }

                break;
        }
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