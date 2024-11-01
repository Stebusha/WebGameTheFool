using Microsoft.AspNetCore.SignalR;

namespace BlazorCardGame.Hubs;

public class GameHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinGame(string playerName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "GameGroup");
        await Clients.Group("GameGroup").SendAsync("Player Joined", playerName);
    }
}