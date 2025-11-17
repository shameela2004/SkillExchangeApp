using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyApp1.Application.DTOs.Group;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Infrastructure.Services;

namespace MyApp1.API.Hubs
{
    [Authorize]
    public class GroupChatHub : Hub
    {
        private readonly IGroupService _groupService;
        private readonly INotificationService _notificationService;

        public GroupChatHub(IGroupService groupService, INotificationService notificationService)
        {
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            _notificationService = notificationService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            // Add this connection to all user groups on connect
            var userIdInt = int.Parse(userId);
            var groups = await _groupService.GetMyGroups(userIdInt);
            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"group-{group.Id}");
            }

            await base.OnConnectedAsync();
        }

        // User joins a specific group (e.g., when opening group chat window)
        public async Task JoinGroup(int groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"group-{groupId}");
        }

        // User leaves a specific group
        public async Task LeaveGroup(int groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"group-{groupId}");
        }

        // Send a message to a group chat
        //public async Task SendMessageToGroup(int groupId, string messageContent)
        //{
        //    try
        //    {
        //        var senderUserId = Context.UserIdentifier;

        //        if (!int.TryParse(senderUserId, out int senderId))
        //            throw new HubException("Invalid sender id");

        //        var messageDto = new SendGroupMessageDto
        //        {
        //            Content = messageContent
        //        };

        //        // Save message to DB
        //        int messageId = await _groupService.SendMessageAsync(groupId, senderId, messageDto);

        //        // Broadcast message to all clients in the group
        //        await Clients.Group($"group-{groupId}").SendAsync(
        //            "ReceiveGroupMessage",
        //            senderUserId,
        //            messageContent,
        //            groupId,
        //            DateTime.UtcNow);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error sending group message: {ex}");
        //        throw;
        //    }
        //}
        public async Task SendMessageToGroup(int groupId, string messageContent)
        {
            try
            {
                var senderUserId = Context.UserIdentifier;
                if (!int.TryParse(senderUserId, out var senderId))
                    throw new HubException("Invalid sender id");

                var messageDto = new SendGroupMessageDto
                {
                    Content = messageContent
                };

                // Save message
                int messageId = await _groupService.SendMessageAsync(groupId, senderId, messageDto);

                // Create notification message content, e.g., "New message in group X".
                var notificationTitle = "New Group Message";
                var notificationMsg = $"New message in group {groupId}";

                // Save notification in DB for all group members - this might be implemented in service (need retrieval of member userIds).
                // Here for example, notify sender and other group members except sender.
                // You need to fetch group members list in your service or hub here to notify each user.
                var groupMembers = await _groupService.GetGroupMembersAsync(groupId);

                foreach (var member in groupMembers)
                {
                    // Skip notification for sender
                    if (member.UserId == senderId)
                        continue;

                    await _notificationService.CreateNotificationAsync(member.UserId, notificationTitle, notificationMsg, "Group");
                    // Push real-time notification via SignalR
                    await Clients.User(member.UserId.ToString())
                        .SendAsync("ReceiveNotification", notificationMsg);
                }

                // Broadcast group message to connected clients
                await Clients.Group($"group-{groupId}").SendAsync("ReceiveGroupMessage", senderUserId, messageContent, groupId, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending group message: {ex}");
                throw;
            }
        }

        public async Task SendNotification(string userId, string notificationMessage)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", notificationMessage);
        }
      

    }
}
