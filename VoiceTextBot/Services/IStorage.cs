using VoiceTextBot.Models;

namespace VoiceTextBot.Services;

public interface IStorage
{
    Session GetSession(long chatId);
}