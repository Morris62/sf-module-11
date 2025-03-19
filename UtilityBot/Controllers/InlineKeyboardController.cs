using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Models;
using UtilityBot.Services;

namespace UtilityBot.Controllers;

public class InlineKeyboardController(ITelegramBotClient botClient, IStorage memory)
{
    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        Console.WriteLine($"Контроллер {GetType().Name} получил событие нажатия кнопки");

        if (callbackQuery?.Data is null)
            return;

        memory.GetSession(callbackQuery.From.Id).Choice = callbackQuery.Data;

        var choice = callbackQuery.Data switch
        {
            "calcChars" => "Подсчет количества символов в тексте",
            "calcSumma" => "Вычисление суммы чисел",
            _ => String.Empty
        };

        await botClient.SendMessage(callbackQuery.From.Id,
            $"<b>Выбрано действие -  {choice}</b>{Environment.NewLine}" +
            $"{Environment.NewLine}Введите строку",
            cancellationToken: ct, parseMode: ParseMode.Html);
    }
}