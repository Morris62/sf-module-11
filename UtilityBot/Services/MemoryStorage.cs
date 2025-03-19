using System.Collections.Concurrent;
using UtilityBot.Models;

namespace UtilityBot.Services;

public class MemoryStorage : IStorage
{
    private readonly ConcurrentDictionary<long, Session> _sessions = new();

    public Session GetSession(long chatId)
    {
        if (_sessions.TryGetValue(chatId, out var session))
            return session;

        var newSession = new Session { Choice = String.Empty };
        _sessions.TryAdd(chatId, newSession);
        return newSession;
    }
}