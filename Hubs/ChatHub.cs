using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task AddToGroup(string groupName)
        {
            var username = Context.GetHttpContext().Request.Query["username"];
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var user = Context.User.Identity.Name;
            
            await Clients.Group(groupName).SendAsync("echo", $"{username} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public async Task NewMessage(long userId, string message)
        {
            var userName = Context.GetHttpContext().Request.Query["username"];
            var groupName = Context.GetHttpContext().Request.Query["room"];
            await Clients.Group(groupName).SendAsync("messageReceived", userId, userName, message);
        }
        public async Task busyTyping(long userId)
        {
            var userName = Context.GetHttpContext().Request.Query["username"];
            var groupName = Context.GetHttpContext().Request.Query["room"];
            await Clients.Group(groupName).SendAsync("userTyping", userId, userName);
        }
    }
}