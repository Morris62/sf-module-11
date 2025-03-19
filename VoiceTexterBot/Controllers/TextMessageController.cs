using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace VoiceTexterBot.Controllers;

public class TextMessageController(ITelegramBotClient botClient)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        switch (message.Text)
        {
            case "/start":
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData("Русский", "ru"),
                    InlineKeyboardButton.WithCallbackData("English", "en"),
                    InlineKeyboardButton.WithCallbackData("Français", "fr")
                });
                await botClient.SendMessage(message.Chat.Id,
                    $"<b>Наш бот превращает аудио в текст</b>{Environment.NewLine}" +
                    $"{Environment.NewLine}Можно записать сообщение и переслать другу, если лень печатать.{Environment.NewLine}",
                    cancellationToken: cancellationToken, parseMode: ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
                break;
            default:
                await botClient.SendMessage(message.Chat.Id, "Отправьте аудио для превращения в текст.",
                    cancellationToken: cancellationToken);
                break;
        }
    }
}