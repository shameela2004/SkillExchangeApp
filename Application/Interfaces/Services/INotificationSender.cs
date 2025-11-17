using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface INotificationSender
    {
        Task SendNotification(string userId, string notificationMessage);
    }
}
