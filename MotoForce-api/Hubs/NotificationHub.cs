using Microsoft.AspNetCore.SignalR;

namespace MotoForce_api.Hubs;

public class NotificationHub: Hub
{
    
    public override Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext().Request.Query["userId"];
        if (!string.IsNullOrEmpty(userId))
        {
            Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.GetHttpContext().Request.Query["userId"];
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}