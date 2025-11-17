using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MyApp1.API.Hubs
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
