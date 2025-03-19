using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;
using UtilityBot.Utilities;

namespace UtilityBot.Controllers;

public class TextMessageController(ITelegramBotClient botClient, IStorage memory)
{
    public async Task Handle(Message message, CancellationToken ct)
    {
        switch (message.Text)
        {
            case "/start":
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData("Подсчёт символов", "calcChars"),
                    InlineKeyboardButton.WithCallbackData("Вычисление суммы", "calcSumma")
                });

                await botClient.SendMessage(message.Chat.Id,
                    $"<b>Бот способен подсчитать количество символов в тексте, а также вычислять сумму чисел." +
                    $"{Environment.NewLine}Выберите действие</b>" +
                    $"{Environment.NewLine}", cancellationToken: ct,
                    parseMode: ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
                break;
            default:
                var choice = memory.GetSession(message.Chat.Id).Choice;
                if (String.IsNullOrEmpty(choice))
                {
                    await botClient.SendMessage(message.Chat.Id, "Вернитесь в главное меню и выберите действие",
                        cancellationToken: ct);
                    break;
                }

                var result = choice switch
                {
                    "calcChars" => MessageLengthCalculator.Process(message.Text),
                    "calcSumma" => SummaCalculator.Process(message.Text),
                    _ => String.Empty
                };
                
                await botClient.SendMessage(message.Chat.Id, result, cancellationToken: ct);
                break;
        }
    }
}