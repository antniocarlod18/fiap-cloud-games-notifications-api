using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesNotifications.Application.Services.Interfaces
{
    public interface IUserNotificationProfileService
    {
        Task CreateUserNotificationProfileAsync(Guid userId, string email);
    }
}
