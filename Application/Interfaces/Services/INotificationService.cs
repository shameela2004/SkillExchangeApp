using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task<int> CreateNotificationAsync(int userId, string title, string message, string type);
        Task<bool> MarkNotificationAsReadAsync(int notificationId);

    }
}
