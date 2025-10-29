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
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId);

    }
}
