using FiapCloudGamesNotifications.Application.Services.Interfaces;
using System.Security.Claims;

namespace FiapCloudGamesNotifications.Api.Endpoints
{
    public static class NotificationsEndpoints
    {
        public static IEndpointRouteBuilder MapNotificationEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/users/{userId}/notifications", GetByUserIdAsync)
                .RequireAuthorization("SameUserOrAdmin");

            return endpoints;
        }

        public static async Task<IResult> GetByUserIdAsync(Guid userId, INotificationService service)
        {
            var list = await service.GetByUserIdAsync(userId);
            return Results.Ok(list);
        }
    }
}
