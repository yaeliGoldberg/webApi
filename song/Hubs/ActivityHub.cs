using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace SONG.Hubs;

public class ActivityHub : Hub
{
    private static readonly ConcurrentDictionary<string, HashSet<string>> _userConnections = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst("userid")?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            _userConnections.AddOrUpdate(userId,
                new HashSet<string> { Context.ConnectionId },
                (key, existing) =>
                {
                    existing.Add(Context.ConnectionId);
                    return existing;
                });
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst("userid")?.Value;
        if (!string.IsNullOrEmpty(userId) && _userConnections.TryGetValue(userId, out var connections))
        {
            connections.Remove(Context.ConnectionId);
            if (connections.Count == 0)
            {
                _userConnections.TryRemove(userId, out _);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public static IEnumerable<string> GetUserConnections(string userId)
    {
        return _userConnections.TryGetValue(userId, out var connections) ? connections : Enumerable.Empty<string>();
    }
}