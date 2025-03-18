using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTextBot.Configuration;
using VoiceTextBot.Services;

namespace VoiceTextBot.Controllers;

public class VoiceMessageController(
    ITelegramBotClient botClient,
    IFileHandler audioFileHandler,
    IStorage memoryStorage)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var fileId = message.Voice?.FileId;
        if (fileId == null)
            return;

        await audioFileHandler.Download(fileId, cancellationToken);

        await botClient.SendMessage(message.Chat.Id, "Голосовое сообщение загружено",
            cancellationToken: cancellationToken);

        string userLanguageCode = memoryStorage.GetSession(message.Chat.Id).LanguageCode;
        var result = audioFileHandler.Process(userLanguageCode);
        await botClient.SendMessage(message.Chat.Id, result,
            cancellationToken: cancellationToken);
    }
}