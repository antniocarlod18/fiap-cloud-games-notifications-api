using FiapCloudGamesNotifications.Domain.Entities;
using FiapCloudGamesNotifications.Domain.Repositories;
using FiapCloudGamesNotifications.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGamesNotifications.Infra.Data.Repositories;

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    private readonly ContextDb _context;

    public NotificationRepository(ContextDb contextDb) : base(contextDb)
    {
        this._context = contextDb;
    }

    public async Task<IList<Notification>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(g => g.UserId == userId)
            .OrderByDescending(n => n.DateCreated)
            .ToListAsync();
    }
}
