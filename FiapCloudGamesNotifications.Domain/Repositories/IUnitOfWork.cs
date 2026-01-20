namespace FiapCloudGamesNotifications.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    INotificationRepository NotificationsRepo { get; }
    IUserNotificationProfileRepository UserNotificationProfileRepo { get; }
    Task Commit();
}
