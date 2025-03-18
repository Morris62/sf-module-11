using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceTextBot.Controllers;

namespace VoiceTextBot;

public class Bot : BackgroundService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly DefaultMessageController _defaultMessageController;
    private readonly TextMessageController _textMessageController;
    private readonly VoiceMessageController _voiceMessageController;
    private readonly InlineKeyboardController _inlineKeyboardController;

    public Bot(ITelegramBotClient telegramBotClient, DefaultMessageController defaultMessageController,
        TextMessageController textMessageController, VoiceMessageController voiceMessageController,
        InlineKeyboardController inlineKeyboardController)
    {
        _telegramBotClient = telegramBotClient;
        _defaultMessageController = defaultMessageController;
        _textMessageController = textMessageController;
        _voiceMessageController = voiceMessageController;
        _inlineKeyboardController = inlineKeyboardController;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramBotClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions() { AllowedUpdates = { } },
            stoppingToken);

        Console.WriteLine("Бот запущен");

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Обработчик обычных событий (нажатие кнопки, отправка сообщения)
    /// </summary>
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken: cancellationToken);
            return;
        }

        if (update.Type == UpdateType.Message)
        {
            switch (update.Message.Type)
            {
                case MessageType.Text:
                    await _textMessageController.Handle(update.Message, cancellationToken);
                    return;
                case MessageType.Voice:
                    await _voiceMessageController.Handle(update.Message, cancellationToken);
                    return;
                default:
                    await _defaultMessageController.Handle(update.Message, cancellationToken);
                    return;
            }
        }
    }

    /// <summary>
    ///     Обработчик ошибок
    /// </summary>
    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}]",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);

        Console.WriteLine("Ожидание 10 секунд перед повторным подключением");
        Thread.Sleep(10_000);

        return Task.CompletedTask;
    }
}