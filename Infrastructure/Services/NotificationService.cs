using AutoMapper;
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
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepository<Notification> _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(IGenericRepository<Notification> notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _notificationRepository.Table
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> CreateNotificationAsync(int userId, string title, string message, string type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title ?? "New Notification",
                Message = message,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                return false;
            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            await _notificationRepository.SaveChangesAsync();
            return true;
        }
    }
}
