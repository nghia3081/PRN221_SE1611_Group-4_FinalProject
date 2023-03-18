using Microsoft.AspNetCore.SignalR;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Services
{
    public class ChatHub : Hub
    {
        private readonly MotelManagementContext _context;

        public ChatHub(MotelManagementContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string message)
        {
            string connectionId = Context.ConnectionId;
            string userId = Context.User.Identity.Name;

            var user = _context.Users.Find(userId);
            var chatMessage = new ChatMessage
            {   Id = Guid.NewGuid(),
                user = user,
                Text = message
            };

            await Clients.Others.SendAsync("ReceiveMessage", userId, message);
        }
    }
}
