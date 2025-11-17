using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyApp1.Application.DTOs.Message;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;

namespace MyApp1.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            Console.WriteLine($"User {userId} connected and added to group {userId}");
            await base.OnConnectedAsync();
        }

        //public async Task SendMessageToUser(string receiverUserId, string messageContent)
        //{

        //    var senderUserId = Context.UserIdentifier;
        //    Console.WriteLine($"SendMessageToUser called from {senderUserId} to {receiverUserId} with message: {messageContent}");

        //    if (!int.TryParse(senderUserId, out int senderId) || !int.TryParse(receiverUserId, out int receiverId))
        //        throw new HubException("Invalid user id");

        //    var messageDto = new MessageCreateDto
        //    {
        //        ToUserId = receiverId,
        //        Content = messageContent
        //        // VoiceFilePath and FilePath can be added if needed from frontend
        //    };

        //    // Save message using service with sender ID and dto
        //    await _messageService.CreateMessageAsync(senderId, messageDto);

        //    // Send the message to both sender and receiver groups
        //    await Clients.Group(senderUserId).SendAsync("ReceiveMessage", senderUserId, messageContent);
        //    await Clients.Group(receiverUserId).SendAsync("ReceiveMessage", senderUserId, messageContent);
        //}
        public async Task SendMessageToUser(int receiverUserId, string messageContent)
        {
            try
            {
                var senderUserId = Context.UserIdentifier;
                Console.WriteLine($"🔥 HUB SENDER ID: {senderUserId}");
                Console.WriteLine($"🔥 HUB RECEIVER ID: {receiverUserId}");
                Console.WriteLine($"🔥 HUB MESSAGE: {messageContent}");

                if (!int.TryParse(senderUserId, out int senderId))
                    throw new HubException("Invalid sender id");

                var messageDto = new MessageCreateDto
                {
                    ToUserId = receiverUserId,
                    Content = messageContent
                };

                var saved = await _messageService.CreateMessageAsync(senderId, messageDto);
                if (!saved) throw new HubException("Failed to save message");

                await Clients.Group(senderUserId).SendAsync("ReceiveMessage", senderUserId, messageContent);
                await Clients.Group(receiverUserId.ToString()).SendAsync("ReceiveMessage", senderUserId, messageContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 ERROR IN HUB:");
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task SendVoiceMessage(int receiverUserId, string voiceUrl)
{
    var senderUserId = Context.UserIdentifier;
            if (!int.TryParse(senderUserId, out int senderId))
                throw new HubException("Invalid sender id");
            var dto = new MessageCreateDto
    {
        ToUserId = receiverUserId,
        VoiceFilePath = voiceUrl   // <--- only URL, not IFormFile
    };

    var saved =await _messageService.CreateMessageAsync(senderId, dto);

    await Clients.Group(receiverUserId.ToString())
        .SendAsync("ReceiveVoiceMessage", senderUserId, voiceUrl);

    await Clients.Group(senderUserId)
        .SendAsync("ReceiveVoiceMessage", senderUserId, voiceUrl);
}



    }
}
