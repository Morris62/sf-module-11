using Telegram.Bot;
using Telegram.Bot.Types;

namespace UtilityBot.Controllers;

public class DefaultMessageController(ITelegramBotClient botClient)
{
    public async Task Handle(Message message, CancellationToken ct)
    {
        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
        await botClient.SendMessage(message.Chat.Id, "Получено сообщение не поддерживаемого формата",
            cancellationToken: ct);
    }
}