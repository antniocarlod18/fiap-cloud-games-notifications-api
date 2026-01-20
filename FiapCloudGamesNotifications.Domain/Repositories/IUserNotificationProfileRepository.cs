using FiapCloudGamesNotifications.Domain.Entities;

namespace FiapCloudGamesNotifications.Domain.Repositories;

public interface IUserNotificationProfileRepository : IRepository<UserNotificationProfile>
{
    Task<UserNotificationProfile?> GetByUserAsync(Guid userId);
}