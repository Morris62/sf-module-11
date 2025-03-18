using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceTextBot.Services;

namespace VoiceTextBot.Controllers;

public class InlineKeyboardController(ITelegramBotClient botClient, IStorage memoryStorage)
{
    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery?.Data == null)
            return;

        memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;

        string languageCode = callbackQuery.Data switch
        {
            "ru" => "Русский",
            "en" => "Английский",
            "fr" => "Французский",
            _ => String.Empty
        };

        await botClient.SendMessage(callbackQuery.From.Id,
            $"<b>Язык аудио - {languageCode}.</b>{Environment.NewLine}" +
            $"{Environment.NewLine}Можно поменять в главном меню.",
            cancellationToken: cancellationToken,
            parseMode: ParseMode.Html);
    }
}