using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Message;
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
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(IGenericRepository<Message> messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(int user1Id, int user2Id)
        {
            return await _messageRepository.Table
                .Where(m => (m.FromUserId == user1Id && m.ToUserId == user2Id) ||
                            (m.FromUserId == user2Id && m.ToUserId == user1Id))
                .OrderBy(m => m.CreatedAt) // Assuming BaseEntity has CreatedAt
                .ToListAsync();
        }

        public async Task<bool> CreateMessageAsync(int fromUserId, MessageCreateDto dto)
        {
            var message = new Message
            {
                FromUserId = fromUserId,
                ToUserId = dto.ToUserId,
                Content = string.IsNullOrEmpty(dto.VoiceFilePath) ? dto.Content : null,
                VoiceFilePath = dto.VoiceFilePath,
                CreatedAt = DateTime.UtcNow
            };
            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();
            return true;
        }

    }

}
