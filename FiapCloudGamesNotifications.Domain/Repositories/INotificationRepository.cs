using FiapCloudGamesNotifications.Domain.Entities;

namespace FiapCloudGamesNotifications.Domain.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IList<Notification>> GetByUserIdAsync(Guid userId);
}