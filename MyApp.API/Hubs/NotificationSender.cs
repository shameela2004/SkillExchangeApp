using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyApp1.Application.Interfaces.Services;
using System.Threading.Tasks;

namespace MyApp1.API.Hubs
{
    public class NotificationSender : INotificationSender
    {
        private readonly IHubContext<GroupChatHub> _hubContext;

        public NotificationSender(IHubContext<GroupChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendNotification(string userId, string notificationMessage)
        {
            return _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notificationMessage);
        }
    }

}
