using FiapCloudGamesNotifications.Domain.Entities;
using FiapCloudGamesNotifications.Domain.Repositories;
using FiapCloudGamesNotifications.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesNotifications.Infra.Data.Repositories;

public class UserNotificationProfileRepository : Repository<UserNotificationProfile>, IUserNotificationProfileRepository
{
    private readonly ContextDb _context;

    public UserNotificationProfileRepository(ContextDb contextDb) : base(contextDb)
    {
        this._context = contextDb;
    }

    public async Task<UserNotificationProfile?> GetByUserAsync(Guid userId)
    {
        return await _context.UserNotificationProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.UserId == userId);
    }
}
