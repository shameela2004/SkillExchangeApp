using Microsoft.EntityFrameworkCore;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.Services
{
    public class NotificationService :INotificationService
    {
        private readonly IGenericRepository<Notification> _notificationRepository;
        public NotificationService(IGenericRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int userId)
        {
            return await _notificationRepository.Table
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

    }
}
