using MyApp1.Application.DTOs.Message;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesAsync(int user1Id, int user2Id);
        Task<bool> CreateMessageAsync(int fromUserId, MessageCreateDto dto);
    }

}
