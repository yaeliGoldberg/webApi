using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SONG.Hubs
{
    public class UserHub : Hub
    {
        public static Dictionary<string, List<string>> UserConnections = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier ?? "anonymous";

            if (!UserConnections.ContainsKey(userId))
                UserConnections[userId] = new List<string>();

            UserConnections[userId].Add(Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception? exception)
        {
            var userId = Context.UserIdentifier ?? "anonymous";

            if (UserConnections.ContainsKey(userId))
            {
                UserConnections[userId].Remove(Context.ConnectionId);

                if (UserConnections[userId].Count == 0)
                    UserConnections.Remove(userId);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}