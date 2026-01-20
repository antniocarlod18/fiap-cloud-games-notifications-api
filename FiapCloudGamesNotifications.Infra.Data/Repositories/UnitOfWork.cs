using FiapCloudGamesNotifications.Domain.Repositories;
using FiapCloudGamesNotifications.Infra.Data.Context;

namespace FiapCloudGamesNotifications.Infra.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ContextDb _context;
    private INotificationRepository _notificationRepository;
    private IUserNotificationProfileRepository _userNotificationProfileRepository;

    public UnitOfWork(ContextDb contextDb)
    {
        _context = contextDb;
    }

    public INotificationRepository NotificationsRepo
    {
        get
        {
            if (_notificationRepository == null)
            {
                _notificationRepository = new NotificationRepository(_context);
            }
            return _notificationRepository;
        }
    }

    public IUserNotificationProfileRepository UserNotificationProfileRepo
    {
        get
        {
            if (_userNotificationProfileRepository == null)
            {
                _userNotificationProfileRepository = new UserNotificationProfileRepository(_context);
            }
            return _userNotificationProfileRepository;
        }
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
