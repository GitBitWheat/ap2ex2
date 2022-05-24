using Microsoft.AspNetCore.SignalR;
using Services;

namespace ap2ex2.Hubs;

public class MessageHub : Hub
{
    private IUserService _service;
    public MessageHub(IUserService userService)
    {
        _service = userService;
    }

    public async Task sendToUser(string user, string userToSend, string message)
    {
        _service.SendMessage(message, user, userToSend);
        await Clients.User(userToSend).SendAsync("MessageReceived", user, message);
    }
    
    public string GetConnectionId() => Context.ConnectionId;
}