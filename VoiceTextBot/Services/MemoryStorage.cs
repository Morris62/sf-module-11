using System.Collections.Concurrent;
using VoiceTextBot.Models;

namespace VoiceTextBot.Services;

public class MemoryStorage : IStorage
{
    private readonly ConcurrentDictionary<long, Session> _sessions;

    public MemoryStorage()
    {
        _sessions = new ConcurrentDictionary<long, Session>();
    }

    public Session GetSession(long chatId)
    {
        if (_sessions.ContainsKey(chatId))
            return _sessions[chatId];

        var newSession = new Session { LanguageCode = "ru" };
        _sessions.TryAdd(chatId, newSession);
        return newSession;
    }
}